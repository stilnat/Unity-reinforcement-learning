using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// We assume a simple dynamics, cards always have equiprobability to be chosen, i.e infinite number of decks.
/// </summary>
public class SimpleBlackjack : MonoBehaviour
{
    private enum states
    {
        four,
        five,
        six,
        seven,
        eight,
        nine,
        ten,
        eleven,
        twelse,
        thirteen,
        fourteen,
        fifteen,
        sixteen,
        seventeen,
        eighteen,
        nineteen,
        twenty,
        twentyOne,
        twentyTwo,
        twentyThree,
        twentyFour,
        twentyFive,
        twentySix,
        twentySeven,
        twentyEight,
        twentyNine,
        thirty,
        end
    }

    private enum actions
    {
        hit,
        stay
    }



    private int BlackJackReward(int state)
    {
        if(state > 17)
        {
            if (state != 27) return -500;
            else return 0;
        }
        else
        {
            return state*state+1;
        }
    }

    private int FindFirst(float[] array, float value)
    {
        for(int i =0; i< array.Length; i++)
        {
            if(array[i] == value)
            {
                return i;
            }
        }
        return -1;
    }

    // Start is called before the first frame update
    void Start()
    {
        List<SystemDynamic.SingleDynamic> dynamics = new List<SystemDynamic.SingleDynamic>();

        float[] rewards = new float[28];

        for(int r =0; r<28; r++)
        {
            rewards[r] = BlackJackReward(r);
        }


        // When staying, always go to end 
        for(int s =0; s<27; s++)
        {
          dynamics.Add(new SystemDynamic.SingleDynamic(s, (int)actions.stay, FindFirst(rewards, rewards[s]) , (int) states.end, 1));
        }

        // first cards, from four to ten.
        for (int s = 0; s < 7; s++)
        {
            for (int j = s + 2; j < s + 11; j++)
            {
                if (j != s + 10)
                {
                    dynamics.Add(new SystemDynamic.SingleDynamic(s, (int)actions.hit, FindFirst(rewards, rewards[s]), j, 4f / 52f));
                }
                else
                {
                    dynamics.Add(new SystemDynamic.SingleDynamic(s, (int)actions.hit, FindFirst(rewards, rewards[s]), j, 16f / 52f));
                }

            }
        }

        // Issue with soft ace not handled
        for (int s = 7; s < 18; s++)
        {
            for (int j = s + 1; j < s + 10; j++)
            {
                if (j != s + 10)
                {
                    dynamics.Add(new SystemDynamic.SingleDynamic(s, (int)actions.hit, FindFirst(rewards, rewards[s]), j, 4f / 52f));
                }
                else
                {
                    dynamics.Add(new SystemDynamic.SingleDynamic(s, (int)actions.hit, FindFirst(rewards, rewards[s]), j, 16f / 52f));
                }

            }
        }

        dynamics.Add(new SystemDynamic.SingleDynamic((int) states.end, (int)actions.hit, FindFirst(rewards, rewards[(int)states.end]), (int)states.end, 1));
        dynamics.Add(new SystemDynamic.SingleDynamic((int)states.end, (int)actions.stay, FindFirst(rewards, rewards[(int)states.end]), (int)states.end, 1));

        SystemDynamic systemDynamic = new SystemDynamic(dynamics);

        float[] uniqueRewards = new float[20];

        for(int i =0; i<19; i++)
        {
            uniqueRewards[i] = rewards[i];
        }
        uniqueRewards[19] = 0;

        MarkovDecisionProcess mdp = new MarkovDecisionProcess(systemDynamic, new RewardVector(uniqueRewards), 1);

        var res = mdp.FindOptimalPolicy(new float[systemDynamic.getStateNumber()]);
        Policy bestPolicy = res.Item1;
        Debug.Log("BEST POLICY PRINTING *****************************************************************");
        bestPolicy.Print();


    }
}
