using UnityEngine;
using CodeBattle.Core;

namespace CodeBattle
{
    /// <summary>Reads Assets/Resources/challenges.json. Add questions by editing that file.</summary>
    public static class ChallengeLoader
    {
        public static ChallengeBank Load(string resourceName = "challenges")
        {
            TextAsset asset = Resources.Load<TextAsset>(resourceName);
            if (asset == null)
            {
                Debug.LogError("Could not find Resources/" + resourceName + ".json");
                return null;
            }
            ChallengeFile file = JsonUtility.FromJson<ChallengeFile>(asset.text);
            return new ChallengeBank(file);
        }
    }
}
