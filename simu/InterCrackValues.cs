using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace simu
{
    class InterCrackValues
    {
        public int operatorProducticityMLO = 4;
        public int operatorProductivityOthers = 3;
        public int operatorTime = 6;
        public int operatorWeeklyTime = 40;
        public int ppMachineProductivity = 30000;
        public int ppMachineCost = 100;
        public int machineTime = 6;
        public int machineCost = 50;
        public int mlo = 3500;
        public int directAttach = 5600;
        public int interposer = 3700;
        public int hybrid = 3700;
        public int spaceTransformer = 3700;
        public double componentsCost = 0.3;
        public double ballCostMLO = 0.005;
        public double costOfReplacement = 0.43;
        public double costOfRemoval = 0.22;
        public double euroToDollar = 1.21;
        public double operatorHourlyCost;
        public double numberOfComponents;
        public double operatorCostSoldering;
        public double ppMachineCostSoldering;
        public double materialCostSoldering;
        public double totalCostSoldering;
        public double operatorCostBoardMLO;
        public double materialCostBoardBalls;
        public double materialCostBoardMLO;
        public double totalCostBoardMLO;
        public double operatorCostDirect;
        public double materialCostDirect;
        public double totalCostDirect;
        public double operatorCostInterposer;
        public double materialCostInterposer;
        public double totalCostInterposer;
        public double operatorCostHybrid;
        public double materialCostHybrid;
        public double totalCostHybrid;
        public double operatorCostSpace;
        public double materialCostSpace;
        public double totalCostSpace;
        public double operatorCostPC;
        public double machineCostPC;
        public double totalCostPC;
        public double totalMLO;
        public double weeklyMLOCracks = 2;
        public double weeklyMLOCost;
        public double yearlyMLOCost;
        public double solderingModel;
        public double boardModel;
        public double pcModel;
    }
}
