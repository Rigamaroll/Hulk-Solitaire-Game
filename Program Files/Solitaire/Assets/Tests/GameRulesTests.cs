using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class GameRulesTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void GameRulesTestsSimplePasses()
    {
        // Use the Assert class to test conditions
        //         TheLogger.PrintLog("// SOL 12 - Tableau cards can only be stacked in alternating colors");
        // TheLogger.PrintLog("----- Test for alternating colours -----");
        // TheLogger.PrintLog("Cards are alternating suits: " + GameRules.IsAlternating(bottom0, "D4"));
        // TheLogger.PrintLog("Cards are alternating suits: " + GameRules.IsAlternating(bottom0, "H3"));
        // TheLogger.PrintLog("Cards are alternating suits: " + GameRules.IsAlternating(bottom0, "C1"));
        // TheLogger.PrintLog("Cards are alternating suits: " + GameRules.IsAlternating(bottom0, "S13"));
        // TheLogger.PrintLog("// SOL 9 - Rank of cards must be functional as per rules");
        // TheLogger.PrintLog("----- Test for rank -----");
        // TheLogger.PrintLog("Cards can be stacked: bottom: " + GameRules.IsRankGoood(bottom0, "D4", "bottom"));
        // TheLogger.PrintLog("Cards can be stacked: bottom: " + GameRules.IsRankGoood(bottom0, "D6", "bottom"));
        // TheLogger.PrintLog("Cards can be stacked: bottom: " + GameRules.IsRankGoood(bottom0, "D8", "bottom"));
        // TheLogger.PrintLog("Cards can be stacked: bottom: " + GameRules.IsRankGoood(bottom0, "D10", "bottom"));
        // TheLogger.PrintLog("Cards can be stacked: top: " + GameRules.IsRankGoood(bottom0, "D4", "top"));
        // TheLogger.PrintLog("Cards can be stacked: top: " + GameRules.IsRankGoood(bottom0, "D6", "top"));
        // TheLogger.PrintLog("Cards can be stacked: top: " + GameRules.IsRankGoood(bottom0, "D8", "top"));
        // TheLogger.PrintLog("Cards can be stacked: top: " + GameRules.IsRankGoood(bottom0, "D10", "top"));
        // TheLogger.PrintLog(GameRules.IsCardCorrect(bottom0[0], "bottom"));
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator GameRulesTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
