using System;
using System.Collections.Generic;
using GoapAi;
using GoapAstarAi;

namespace ConsoleInterface
{
    public class MainToTestGoap
    {
        public static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            logger.SetLogLevel(LogLevel.None);

            //! Create an Agent and add the possible actions
            var agent = new Agent(logger);
            agent.AddAction(GetGunAction());
            agent.AddAction(PickUpAction());
            agent.AddAction(ShootAction());

            //! Set a goal for the agent
            var goal = KillEnemyGoal();
            agent.AddGoal(0, goal);

            //! Set current state of the agent
            agent.SetFact("EnemyDead", false);
            agent.SetFact("HasGun", false);
            agent.SetFact("ReachedGun", false);
            agent.SetFact("Walk", true);

            //! Get the plan of the agent
            var goapPlanner = GoapPlannerFactory.GetAGoapPlanner(logger);
            Console.WriteLine("\n Original Plan");
            PrintPath(goapPlanner.FindPlanForAgent(agent), goal);

            //! Change the state of the agent 
            agent.SetFact("EnemyDead", false);
            agent.SetFact("HasGun", true);
            agent.SetFact("ReachedGun", false);
            agent.SetFact("Walk", false);

            //! Get the plan of the agent
            Console.WriteLine("\n Plan for updated state");
            PrintPath(goapPlanner.FindPlanForAgent(agent), goal);
        }

        private static void PrintPath(IList<GoapAction> path, GoapGoal goal)
        {
            Console.WriteLine($"Goap Plan");
            foreach (var step in path)
            {
                var action = step;
                Console.Write($"{action.ActionName} -> ");
            }
            Console.WriteLine($" <{goal.GoalName}>");
        }
        private static GoapAction GetGunAction()
        {
            var getGun = GetAction("getGun");
            getGun.PreConditions.SetFact("Walk", true);
            getGun.Effects.SetFact("ReachedGun", true);
            return getGun;
        }

        private static GoapAction PickUpAction()
        {
            var pickUp = GetAction("pickUp");
            pickUp.PreConditions.SetFact("ReachedGun", true);
            pickUp.Effects.SetFact("HasGun", true);
            return pickUp;
        }

        private static GoapAction ShootAction()
        {
            var shoot = GetAction("shoot");
            shoot.PreConditions.SetFact("HasGun", true);
            shoot.Effects.SetFact("EnemyDead", true);
            return shoot;
        }

        private static GoapAction GetAction(string name)
        {
            return new GoapAction(name)
            {
                PreConditions = new State($"p-{name}"),
                Effects = new State($"e-{name}"),
                Cost = 1
            };
        }

        private static GoapGoal KillEnemyGoal()
        {
            var goal = new GoapGoal("KillEnemy");

            goal.SetAGoalFact("EnemyDead", true);
            return goal;
        }

    }
}