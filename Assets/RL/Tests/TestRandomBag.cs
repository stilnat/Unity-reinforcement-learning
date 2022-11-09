using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;

public class TestRandomBag
{
    [Test]
    public void TestRandomBagWithDictionary()
    {
        Dictionary<int, float> keyValues = new Dictionary<int, float>();
        keyValues.Add(0, 0.2f);
        keyValues.Add(1, 0.3f);
        keyValues.Add(2, 0.1f);
        keyValues.Add(3, 0.1f);
        keyValues.Add(4, 0.3f);
        var freq = new float[5] { 0, 0, 0, 0, 0 };
        int res;
        int total = 15000;

        for(int i=0; i< total; i++)
        {
            res = keyValues.RandomElementByWeight(x => x.Value).Key;
            freq[res] += 1;
        }

        for (int i = 0; i < 5; i++)
        {
            freq[i] = freq[i] / total;
        }

        Assert.IsTrue(freq[0] <= 0.21 && freq[0] >= 0.19 && freq[1] <= 0.31 && freq[1] >= 0.29 && freq[2] <= 0.11 && freq[2] >= 0.09
            && freq[3] <= 0.11 && freq[3] >= 0.09 && freq[4] <= 0.31 && freq[4] >= 0.29, "expected frequencies close to freq[0] = 0.2f, freq[1] = 0.3f, freq[2] = 0.1f, freq[3] = 0.1f, freq[4] = 0.3f," +
            " got freq[0] = " + freq[0]+", freq[1] = " +freq[1] +", freq[2] = " + freq[2] +", freq[3] = " + freq[3] +", freq[4] = " + freq[4] );
       
    }
}
