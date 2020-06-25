using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;

public class penguinModel : MonoBehaviour
{
    const double r = 1e-1;
    const double criticalDelta = 0.8;
    const double criticalValue = 3.0;

    static Dictionary<string, act> allActs = new Dictionary<string, act>();
    static Dictionary<string, act> allActsIndependentPinguin = new Dictionary<string, act>();

    static public double[] penguinAppraisals = new double[3];
    static public double[] humanAppraisals = new double[3];
    static public double[] penguinBodyFactor = new double[5];

    double TimerToEat = 0;
    bool timetoeat = false;


    private void FixedUpdate()
    {
        for(int i = 0; i < 5; i++)
        {
            penguinBodyFactor[i] -= 0.0001;
        }

        //foreach (var el in penguinBodyFactor)
        //{
        //    Debug.Log(el);
        //}

        //Debug.Log(penguinBodyFactor[(int)body.satiety]);
        //Debug.Log(TimerToEat);
        Debug.Log(penguinBodyFactor[(int)body.sociability]);
        if(penguinBodyFactor[(int)body.energy] < -criticalValue)
        {
            Debug.Log("f");
            if(GetComponent<penguinAI>().isSleep == false)
            {
                goToSleep();
                GetComponent<penguinAI>().isSleep = true;
                GetComponent<penguinAI>().Sleep(new object());
            }

        }
        if (penguinBodyFactor[(int)body.satiety] > -criticalValue)
            timetoeat = false;

        if (timetoeat)
            TimerToEat += 0.02;
        else
            TimerToEat = 0;



        if (penguinBodyFactor[(int)body.satiety] < -criticalValue && TimerToEat % 30 < 0.5)
        {
            timetoeat = true;
            Debug.Log("f");
            if (GetComponent<penguinAI>().Stop == false)
            {
                goToEat();
                GetComponent<penguinAI>().PenguinActions("goToBoxWithFish");
            }

        }
        

        if (penguinBodyFactor[(int)body.activity] < -criticalValue)
        {
            Debug.Log("f");
            if (GetComponent<penguinAI>().Stop == false)
            {
                goToPlay();
                GetComponent<penguinAI>().PenguinActions("goToBoxWithSnowBalls");
            }

        }

        if (penguinBodyFactor[(int)body.sociability] < -criticalValue)
        {
            Debug.Log("f");
            if (GetComponent<penguinAI>().Stop == false)
            {
                goToCommunicate();
                GetComponent<penguinAI>().PenguinActions("GoCommunicatePenguin");
            }

        }

    }

    enum body
    {
        pain = 0,
        energy = 1,
        satiety = 2,
        activity = 3,
        sociability = 4
    };

    enum moral
    {
        valence = 0,
        arousal = 1,
        dominance = 2
    };
    class act
    {
        private double[] bodyFactorForTarget;
        private double[] moralFactorForTarget;
        private double[] moralFactorForAuthor;

        public act()
        {
            bodyFactorForTarget = new double[5];
            moralFactorForTarget = new double[3];
            moralFactorForAuthor = new double[3];
        }
        public void setBodyFactorForTarget(double[] values)
        {
            for (int i = 0; i < bodyFactorForTarget.Length; ++i)
            {
                bodyFactorForTarget[i] = values[i];
            }
        }
        public void setMoralFactorForTarget(double[] values)
        {
            for (int i = 0; i < moralFactorForTarget.Length; ++i)
            {
                moralFactorForTarget[i] = values[i];
            }
        }
        public void setMoralFactorForAuthor(double[] values)
        {
            for (int i = 0; i < moralFactorForAuthor.Length; ++i)
            {
                moralFactorForAuthor[i] = values[i];
            }
        }
        public double[] getBodyFactorForTarget()
        {
            return bodyFactorForTarget;
        }
        public double[] getMoralFactorForTarget()
        {
            return moralFactorForTarget;
        }

        public double[] getMoralFactorForAuthor()
        {
            return moralFactorForAuthor;
        }



    };

    static double[] getNewBodyFactor(double[] body, double[] action)
    {
        for (int i = 0; i < body.Length; ++i)
        {
            body[i] += action[i];
        }
        return body;
    }

    static double[] getNewAppraisals(double[] appraisals, double[] action)
    {
        for (int i = 0; i < appraisals.Length; ++i)
        {
            appraisals[i] = (1.0 - r) * appraisals[i] + r * action[i];
        }
        return appraisals;
    }


    static public void goToSleep()
    {
        penguinAppraisals = getNewAppraisals(penguinAppraisals, allActsIndependentPinguin["GoSleepPinguin"].getMoralFactorForTarget());
        humanAppraisals = getNewAppraisals(humanAppraisals, allActsIndependentPinguin["GoSleepPinguin"].getMoralFactorForAuthor());
        penguinBodyFactor = getNewBodyFactor(penguinBodyFactor, allActsIndependentPinguin["GoSleepPinguin"].getBodyFactorForTarget());
    }

    static public void goToEat()
    {
        penguinAppraisals = getNewAppraisals(penguinAppraisals, allActsIndependentPinguin["GoEatPenguin"].getMoralFactorForTarget());
        humanAppraisals = getNewAppraisals(humanAppraisals, allActsIndependentPinguin["GoEatPenguin"].getMoralFactorForAuthor());
        penguinBodyFactor = getNewBodyFactor(penguinBodyFactor, allActsIndependentPinguin["GoEatPenguin"].getBodyFactorForTarget());
    }

    static public void goToPlay()
    {
        penguinAppraisals = getNewAppraisals(penguinAppraisals, allActsIndependentPinguin["GoPlayPenguin"].getMoralFactorForTarget());
        humanAppraisals = getNewAppraisals(humanAppraisals, allActsIndependentPinguin["GoPlayPenguin"].getMoralFactorForAuthor());
        penguinBodyFactor = getNewBodyFactor(penguinBodyFactor, allActsIndependentPinguin["GoPlayPenguin"].getBodyFactorForTarget());
    }

    static public void goToCommunicate()
    {
        penguinAppraisals = getNewAppraisals(penguinAppraisals, allActsIndependentPinguin["GoCommunicatePenguin"].getMoralFactorForTarget());
        humanAppraisals = getNewAppraisals(humanAppraisals, allActsIndependentPinguin["GoCommunicatePenguin"].getMoralFactorForAuthor());
        penguinBodyFactor = getNewBodyFactor(penguinBodyFactor, allActsIndependentPinguin["GoCommunicatePenguin"].getBodyFactorForTarget());
    }
    class Penguin
    {
        private double[] bodyFactor;
        private double[] moralFactor;
        public Penguin()
        {
            bodyFactor = new double[5];
            moralFactor = new double[3];
        }

    };


    public void setupActs()
    {
        allActs.Add("HelloPinguin", new act());
        allActs["HelloPinguin"].setBodyFactorForTarget(new double[] { 0, -0.05, 0, 0.3, 0.5 });
        allActs["HelloPinguin"].setMoralFactorForTarget(new double[] { 0.5, 0.05, -0.2 });
        allActs["HelloPinguin"].setMoralFactorForAuthor(new double[] { 0.5, 0.05, -0.3 });

        allActs.Add("FeedPinguin", new act());
        allActs["FeedPinguin"].setBodyFactorForTarget(new double[] { 0, -0.05, 0.7, -0.1, 0.2 });
        allActs["FeedPinguin"].setMoralFactorForTarget(new double[] { 0.5, 0.2, -0.1 });
        allActs["FeedPinguin"].setMoralFactorForAuthor(new double[] { 0.7, 0.15, 0.1 });

        allActs.Add("ThrowBallToPinguin", new act());
        allActs["ThrowBallToPinguin"].setBodyFactorForTarget(new double[] { 0.05, -0.2, -0.1, 0.2, 0.3 });
        allActs["ThrowBallToPinguin"].setMoralFactorForTarget(new double[] { 0.1, 0.5, 0.05 });
        allActs["ThrowBallToPinguin"].setMoralFactorForAuthor(new double[] { 0.1, 0.5, 0.05 });

        allActs.Add("StrokePinguin", new act());
        allActs["StrokePinguin"].setBodyFactorForTarget(new double[] { -0.2, 0, 0, 0.1, 0.7 });
        allActs["StrokePinguin"].setMoralFactorForTarget(new double[] { 0.7, -0.25, -0.2 });
        allActs["StrokePinguin"].setMoralFactorForAuthor(new double[] { 0.7, -0.25, 0.2 });

        allActs.Add("HitPinguin", new act());
        allActs["HitPinguin"].setBodyFactorForTarget(new double[] { 0.7, -0.2, 0, 0, -0.2 });
        allActs["HitPinguin"].setMoralFactorForTarget(new double[] { -1.2, -0.2, -0.5 });
        allActs["HitPinguin"].setMoralFactorForAuthor(new double[] { -0.9, 0.7, 0.5 });

        allActsIndependentPinguin.Add("GoSleepPinguin", new act());
        allActsIndependentPinguin["GoSleepPinguin"].setBodyFactorForTarget(new double[] { -0.2, 0.6, -0.3, -0.2, -0.1 });
        allActsIndependentPinguin["GoSleepPinguin"].setMoralFactorForTarget(new double[] { 0.1, -0.8, 0 });
        allActsIndependentPinguin["GoSleepPinguin"].setMoralFactorForAuthor(new double[] { 0, -0.3, 0 });

        allActsIndependentPinguin.Add("GoPlayPenguin", new act());
        allActsIndependentPinguin["GoPlayPenguin"].setBodyFactorForTarget(new double[] { 0, -0.1, 0, 0.2, 0.2 });
        allActsIndependentPinguin["GoPlayPenguin"].setMoralFactorForTarget(new double[] { 0.6, 0.3, -0.15 });
        allActsIndependentPinguin["GoPlayPenguin"].setMoralFactorForAuthor(new double[] { 0.4, 0.4, -0.05 });

        allActsIndependentPinguin.Add("GoEatPenguin", new act());
        allActsIndependentPinguin["GoEatPenguin"].setBodyFactorForTarget(new double[] { 0, -0.05, 0, 0.1, 0.15 });
        allActsIndependentPinguin["GoEatPenguin"].setMoralFactorForTarget(new double[] { 0.1, 0.2, -0.25 });
        allActsIndependentPinguin["GoEatPenguin"].setMoralFactorForAuthor(new double[] { 0.15, 0.1, 0.15 });

        allActsIndependentPinguin.Add("GoCommunicatePenguin", new act());
        allActsIndependentPinguin["GoCommunicatePenguin"].setBodyFactorForTarget(new double[] { 0, -0.05, 0, 0.1, 0.3 });
        allActsIndependentPinguin["GoCommunicatePenguin"].setMoralFactorForTarget(new double[] { 0.7, 0.2, 0 });
        allActsIndependentPinguin["GoCommunicatePenguin"].setMoralFactorForAuthor(new double[] { 0.5, 0.3, 0 });



        allActsIndependentPinguin.Add("IgnoreHuman", new act());
        allActsIndependentPinguin["IgnoreHuman"].setBodyFactorForTarget(new double[] { 0, 0, 0, 0, -0.3 });
        allActsIndependentPinguin["IgnoreHuman"].setMoralFactorForTarget(new double[] { -0.1, -0.3, 0.15 });
        allActsIndependentPinguin["IgnoreHuman"].setMoralFactorForAuthor(new double[] { -0.1, -0.2, -0.1 });


    }

    static bool firstCriterion(double[] penguinBodyFactor, double[] action)
    {
        double normCurrent = 0, normAfterAction = 0, maxValue = -10.0, minValue = 10.0;
        for (int i = 0; i < penguinBodyFactor.Length; ++i)
        {
            normCurrent += Math.Pow(penguinBodyFactor[i], 2);
            normAfterAction += Math.Pow(penguinBodyFactor[i] + action[i], 2);
            maxValue = Math.Max(maxValue, penguinBodyFactor[i] + action[i]);
            minValue = Math.Min(minValue, penguinBodyFactor[i] + action[i]);
        }
        maxValue = Math.Max(maxValue, Math.Abs(minValue));
        normCurrent = Math.Sqrt(normCurrent);
        normAfterAction = Math.Sqrt(normAfterAction);
        bool almostGood = !(((normAfterAction - normCurrent) < criticalDelta) && (maxValue < criticalValue));//норма сильно не увеличилась и max значение в норме => действие принимаем
        bool excellentMove = !((normAfterAction - normCurrent) < 0);//норма уменьшилась
        return almostGood & excellentMove;
    }

    static bool secondCriterion(double[] penguinMoralFactor, double[] humanMoralFactor)
    {
        double interactionValence = humanAppraisals[(int)moral.valence]
            + penguinAppraisals[(int)moral.valence]
            + Math.Max(0.0, penguinAppraisals[(int)moral.arousal] * 1.3);//если дружеские отношение+коэффициент возбужденности пингвина
        double interactionDominance = (humanAppraisals[(int)moral.dominance] - penguinAppraisals[(int)moral.dominance]) - 0.5;//человек может доминировать и пингвин будет подчиняться
        double interaction = Math.Max(interactionValence, interactionDominance);
        return (interaction + 0.15) > 0;
    }

    public string launchNewScene(string humanAct)
    {
        //foreach (var el in allActs)
        //{
        //    Debug.Log(el.Key);
        //}
        //foreach (var el in penguinAppraisals)
        //{
        //    Debug.Log(el);
        //}

        bool ignore = false;
        string reaction = "positive";
        ignore = firstCriterion(penguinBodyFactor, allActs[humanAct].getBodyFactorForTarget())
            & secondCriterion(penguinAppraisals, humanAppraisals);// чекаем физио состояние пингвина + взаимотношения с человеком

        if (!ignore || humanAct == "HitPinguin")
        {
            penguinAppraisals = getNewAppraisals(penguinAppraisals, allActs[humanAct].getMoralFactorForTarget()); //изменения состояния пингвина после действия
            humanAppraisals = getNewAppraisals(humanAppraisals, allActs[humanAct].getMoralFactorForAuthor()); // изменения состояния человека после действия
            penguinBodyFactor = getNewBodyFactor(penguinBodyFactor, allActs[humanAct].getBodyFactorForTarget()); // изменение физиологический постребностей пингвина
        }
        else
        {
            reaction = "ignore";
            penguinAppraisals = getNewAppraisals(penguinAppraisals, allActsIndependentPinguin["IgnoreHuman"].getMoralFactorForTarget()); //пингвин игнорирует
            humanAppraisals = getNewAppraisals(humanAppraisals, allActsIndependentPinguin["IgnoreHuman"].getMoralFactorForAuthor());
            penguinBodyFactor = getNewBodyFactor(penguinBodyFactor, allActsIndependentPinguin["IgnoreHuman"].getBodyFactorForTarget());
        }
        Debug.Log(reaction);
        return reaction;

    }
}
