using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


namespace SGJ2019
{
	public enum GameState
	{
		ONGOING, WON, LOST
	}


	public class GameManager : MonoBehaviour, IManagedInitialization, IManagedDestroy
	{
		private GameState gameState = GameState.ONGOING;
		public GameState GameState => gameState;
		[SerializeField] private float loadSceneTime = 2.0f;
		[SerializeField] private int roundLimit = 20;
		private List<Card> monsters = new List<Card>();
		private List<Card> soldiers = new List<Card>();
		[SerializeField] private TextBox textBox = null;


		public Dictionary<InitializationPhases, System.Action> InitializationActions =>
				new Dictionary<InitializationPhases, System.Action>()
				{
					[InitializationPhases.FIRST] = ManagedInitialize
				};


		private void ManagedInitialize()
		{
			Assert.IsNotNull(textBox);
			foreach (var card in FindObjectsOfType<Card>())
			{
				TryAddCard(card);
			}
		}

		private void LateUpdate()
		{
			if (gameState == GameState.ONGOING)
			{
				if (monsters.Count == 0)
				{
					gameState = GameState.WON;
					textBox.SetText("Victory:" + "\n" + "All Enemies Defeated", Color.green);
				}
				else if (soldiers.Count == 0)
				{
					gameState = GameState.LOST;
					textBox.SetText("Defeat:" + "\n" + "All Soldiers Lost", Color.red);
				}
				else if (TurnManager.Instance.RoundNumber > roundLimit)
				{
					gameState = GameState.LOST;
					textBox.SetText("Defeat:" + "\n" + "Time Out", Color.red);
				}
			}
			else
			{
				loadSceneTime -= Time.deltaTime;
				if (loadSceneTime <= 0.0f)
				{
					SceneManager.LoadScene("MainMenu");
				}
			}
		}

		public void ManagedDestruction()
		{
			foreach (var monster in monsters)
			{
				monster.LifecycleComponent.OnLifecycleComponentDestroyed -= OnMonsterDestroyed;
			}
			monsters.Clear();
			foreach (var soldier in soldiers)
			{
				soldier.LifecycleComponent.OnLifecycleComponentDestroyed -= OnSoldierDestroyed;			
			}
			soldiers.Clear();
		}

		private void TryAddCard(Card card)
		{
			if (card.Ownership == OwnerPhase.ENEMY)
			{
				if (!monsters.Contains(card))
				{
					monsters.Add(card);
					card.LifecycleComponent.OnLifecycleComponentDestroyed += OnMonsterDestroyed;
				}
			}
			else if (card.Ownership == OwnerPhase.HUMAN)
			{
				if (!soldiers.Contains(card))
				{
					soldiers.Add(card);
					card.LifecycleComponent.OnLifecycleComponentDestroyed += OnSoldierDestroyed;
				}
			}
		}

		private void OnGlobalLifecycleComponentCreated(LifecycleComponent lifecycleComponent)
		{
			var card = lifecycleComponent.GetComponent<Card>();
			if (card != null)
			{
				TryAddCard(card);
			}
		}

		private void OnMonsterDestroyed(LifecycleComponent lifecycleComponent)
		{
			lifecycleComponent.OnLifecycleComponentDestroyed -= OnMonsterDestroyed;
			monsters.Remove(lifecycleComponent.GetComponent<Card>());
		}

		private void OnSoldierDestroyed(LifecycleComponent lifecycleComponent)
		{
			lifecycleComponent.OnLifecycleComponentDestroyed -= OnSoldierDestroyed;
			soldiers.Remove(lifecycleComponent.GetComponent<Card>());
		}
	}
}