using UnityEngine;
using UnityEngine.Assertions;


namespace SGJ2019
{
	public class FloatingNumber : MonoBehaviour
	{
		[SerializeField] private TMPro.TextMeshProUGUI text = null;
		public TMPro.TextMeshProUGUI Text => text;
		[SerializeField] private float fadeoutTimer = 1.0f;
		[SerializeField] private Vector2 floatDirection = new Vector2(0.0f, 50.0f);


		private void Awake()
		{
			Assert.IsNotNull(text);
			Assert.IsTrue(fadeoutTimer > 0.0f);
		}

		private void Update()
		{
			fadeoutTimer -= Time.deltaTime;
			if (fadeoutTimer <= 0.0f)
			{
				Destroy(gameObject);
			}
			else
			{
				transform.position = transform.position + (new Vector3(floatDirection.x, floatDirection.y, 0.0f) * Time.deltaTime);
			}
		}
	}
}
