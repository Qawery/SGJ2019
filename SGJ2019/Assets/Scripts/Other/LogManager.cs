using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


namespace SGJ2019
{
	public class LogManager : SimpleSingleton<LogManager>
	{
		[SerializeField] private int messageLimit = 10;
		[SerializeField] private TMPro.TextMeshProUGUI text = null;
		private List<string> messages = new List<string>();
			

		protected override void ManagedInitialize()
		{
			base.ManagedInitialize();
			if (Instance == this)
			{
				Assert.IsNotNull(text);
				UpdateText();
			}
		}

		public void AddMessage(string message)
		{
			messages.Add(message);
			while (messages.Count > messageLimit)
			{
				messages.RemoveAt(0);
			}
			UpdateText();
		}

		private void UpdateText()
		{
			string finalText = "";
			foreach (var message in messages)
			{
				finalText += "\n" + message;
			}
			text.text = finalText;
		}
	}
}