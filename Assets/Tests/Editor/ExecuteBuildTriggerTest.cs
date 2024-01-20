using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.TestTools;

namespace Tests.Editor
{
	public class ExecuteBuildTriggerTest
	{
		// curl http://81.23.182.126:22829/job/empty/buildWithParameters --user jenkins:1136e41664802d33e78fdcbb5ef4e6ab8f --data token=aD6vpc2Odju8LBjfbyYteGINGQN3296u --data branchName=develop


		[Timeout(5000)]
		[UnityTest]
		public IEnumerator Execute_Build_Trigger_Test()
		{
			var authorizationString =
				Convert.ToBase64String(Encoding.UTF8.GetBytes("jenkins:1136e41664802d33e78fdcbb5ef4e6ab8f"));
			var data = new Dictionary<string, string>
			{
				{ "token", "aD6vpc2Odju8LBjfbyYteGINGQN3296u" },
				{ "branchName", "develop" },
			};
			var request = UnityWebRequest.Post("http://81.23.182.126:22829/job/empty/buildWithParameters", data);
			request.SetRequestHeader("Authorization", $"Basic {authorizationString}");
			var operation = request.SendWebRequest();

			while (!operation.isDone)
			{
				yield return null;
			}

			UnityEngine.Debug.Log($"[{nameof(ExecuteBuildTriggerTest)}] {request.responseCode}");
		}
	}
}