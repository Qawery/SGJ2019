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


	public class GameManager : SimpleSingleton<GameManager>
	{
		[SerializeField] private float loadSceneTime = 2.0f;
		[SerializeField] private int roundLimit = 20;
		public int RoundLimit => roundLimit;
		private List<Card> monsters = new List<Card>();
		private List<Card> soldiers = new List<Card>();
		[SerializeField] private TextBox textBox = null;


		public GameState GameState { get; private set; } = GameState.ONGOING;


		protected override void ManagedInitialize()
		{
			base.ManagedInitialize();
			if (Instance == this)
			{
				Assert.IsNotNull(textBox);
				foreach (var card in FindObjectsOfType<Card>())
				{
					TryAddCard(card);
				}
				LifecycleComponent.GlobalOnLifecycleComponentCreated += OnGlobalLifecycleComponentCreated;
			}
		}

		private void LateUpdate()
		{
			if (GameState == GameState.ONGOING)
			{
				if (monsters.Count == 0)
				{
					GameState = GameState.WON;
					textBox.SetText("Victory:" + "\n" + "All Enemies Defeated", Color.green);
				}
				else if (soldiers.Count == 0)
				{
					GameState = GameState.LOST;
					textBox.SetText("Defeat:" + "\n" + "All Soldiers Lost", Color.red);
				}
				else if (TurnManager.Instance.RoundNumber > roundLimit)
				{
					GameState = GameState.LOST;
					textBox.SetText("Defeat:" + "\n" + "Time Out", Color.red);
				}
			}
			else
			{
				loadSceneTime -= Time.deltaTime;
				if (loadSceneTime <= 0.0f)
				{
					QuitToMainMenu();
				}
			}
		}

		public override void ManagedDestruction()
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
			LifecycleComponent.GlobalOnLifecycleComponentCreated -= OnGlobalLifecycleComponentCreated;
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

		public void QuitToMainMenu()
		{
			SceneManager.LoadScene("MainMenu");
		}
	}
}