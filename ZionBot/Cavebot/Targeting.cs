using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OtClientBot.Cavebot
{
    public class Targeting : Module
    {

        public TargetingModes.TargetingMode targetingMode = null;

        public bool switchToHigherPriority = true;

        public TargetPolicy currentTargetPolicy = null;

        public TargetPolicy defaultTargetPolicy = null;

        public ObservableCollection<TargetPolicy> targetPolicies = new ObservableCollection<TargetPolicy>();
               






        private void Load()
        {
            this.ThreadEntryPoint = TargetingLoop;
        }

        public Targeting(ObservableCollection<TargetPolicy> targetPolicies, TargetPolicy defaultTargetPolicy, bool switchToHigherPriority )
        {
            this.targetPolicies = targetPolicies;
            this.defaultTargetPolicy = defaultTargetPolicy;
            this.switchToHigherPriority = switchToHigherPriority;
            this.currentTargetPolicy = defaultTargetPolicy;


            AtStart += () => { _Cavebot.thereAreTargetsOnScreen = false; };
            AtStop += () => { _Cavebot.thereAreTargetsOnScreen = false; };


            Load();
        }


        private void TargetingLoop()
        {
            long c = 0;
            while (true)
            {
                if (Player.IsOnline)
                {
                    // Se Player não estiver atacando, ou se estiver atacando e o alvo não é reachable.
                    if (!Player.isAttacking || (Player.isAttacking && !Player.AttackingCreature.Location.isReachable()))
                    {
                        if (targetingMode != null) // Desativa o targeting mode case ele esteja ligado
                        {
                            targetingMode.Stop();
                            targetingMode = null;
                        }
                        var target = GetTarget();
                        if (target != null)
                        {
                            _Cavebot.thereAreTargetsOnScreen = true;

                            if (Player.isWalking) {

                                Player.Stop();

                                Location locTo = null;

                                if (target.Location.GetWalkableLocationAround(1, out locTo))
                                    Player.WalkTo(locTo);


                            }


                            Player.AttackCreature(target);
                            Client.WaitPing(500);
                        }
                        else
                        {
                            _Cavebot.thereAreTargetsOnScreen = false;
                        }
                    }
                    else
                    {
                        _Cavebot.thereAreTargetsOnScreen = true;

                        if (Player.AttackingCreature.Location.isReachable()== false)
                        {
                            Player.Stop();
                            Client.WaitPing(200);
                        }


                        if (c % 10 == 0) Player.AttackCreature(Player.AttackingCreature); // Send the packet while attacking..


                        if (targetingMode == null)
                        {
                            targetingMode = GetTargetingMode();
                            targetingMode.Start();
                        }

                        if (switchToHigherPriority) // Se sim, trocará de target para um target com maior prioridade caso haja um na tela.
                        {
                            var target = GetTarget((byte)currentTargetPolicy.priority + 1); // Look for targets with a higher priority
                                                                                            //Verify if Player.AttackingCreature is CreaturePtr or CreatureId
                            if (target != null && target.Id != Player.AttackingCreature.Id)
                            {
                                Player.AttackCreature(target);
                                targetingMode.Stop();
                                targetingMode = null;
                                Client.WaitPing(500);
                            }
                        }

                    }
                }
                
                Thread.Sleep(300);
                c++;

            }
        }

        private TargetingModes.TargetingMode GetTargetingMode()
        {
            switch (this.currentTargetPolicy.targetingMode)
            {
                case Enums.TargetingMode.Follow:
                    return new TargetingModes.Follow();
                case Enums.TargetingMode.Stand:
                    return new TargetingModes.Stand();
                default:
                    return new TargetingModes.None();
            }


            //return new TargetingModes.None();
            //throw new NotImplementedException();
        }


        private Creature GetTargetFromList(List<Creature> Creatures, TargetPolicy policy)
        {
            return 
                (from creature in Creatures
                where (policy.isDefault || creature.Name.ToLower() == policy.name.ToLower() ) && creature.Location.isReachable()
                orderby creature.Location.SquareDistanceTo(Player.Location)
                select creature).FirstOrDefault();


        }


        private Creature GetTargetFromPolicy(TargetPolicy policy)
        {
            // Nome das creatures para ignorar
            var ignoreList = from _policy in this.targetPolicies
                             where _policy.ignore
                             select _policy.name.ToLower();



            // Criaturas que estão na tela e que não estão na ignore list
            var CreaturesOnScreen = (from creature in BattleList.GetAllCreatures(true)
                                     where creature.isCreature() && ignoreList.Contains(creature.Name.ToLower()) == false
                                     select creature).ToList<Creature>();




            var Creature = GetTargetFromList(CreaturesOnScreen, policy);

            if (Creature != null)
            {
                this.currentTargetPolicy = policy;

                return Creature;
            }


 

            return null;
        }



        private Creature GetTarget(int minPriority = 0)
        {   
            
            // Policies
            var Policies = from policy in this.targetPolicies
                           where policy.ignore == false && ((byte)policy.priority >= (byte)minPriority) && !string.IsNullOrWhiteSpace(policy.name) && policy.isNotDefault
                           orderby policy.priority descending
                           select policy;


            // Procura por um target especificado nas policies
            foreach (TargetPolicy policy in Policies)
            {
                var Creature = GetTargetFromPolicy(policy);

                if (Creature != null)
                {
                    return Creature;
                }

                
            }

            // Procura por um target de acordo com a politica default.

            if (!this.defaultTargetPolicy.ignore && (byte)defaultTargetPolicy.priority >= (byte)minPriority)
            {
                var Creature = GetTargetFromPolicy(defaultTargetPolicy);

                if (Creature != null)
                {
                    return Creature;
                }
            }

            return null;
        }
    }
}
