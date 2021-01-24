using System;


namespace simu
{
    class InterCrack
    {
        public InterCrackValues interCrackValues;

        public InterCrack(double numberOfComponents) {
            interCrackValues = new InterCrackValues();
            interCrackValues.numberOfComponents = numberOfComponents;
        }

        public void calculateValues(String interconnectType, String interconnectCost) {
            interCrackValues.operatorHourlyCost = 26 * interCrackValues.euroToDollar;
            interCrackValues.operatorCostSoldering = interCrackValues.operatorHourlyCost * interCrackValues.operatorWeeklyTime / interCrackValues.operatorProducticityMLO;
            interCrackValues.ppMachineCostSoldering = interCrackValues.numberOfComponents * interCrackValues.ppMachineCost / interCrackValues.ppMachineProductivity;
            interCrackValues.materialCostSoldering = interCrackValues.numberOfComponents * interCrackValues.componentsCost;
            interCrackValues.totalCostSoldering = interCrackValues.operatorCostSoldering + interCrackValues.ppMachineCostSoldering + interCrackValues.materialCostSoldering;

            interCrackValues.operatorCostBoardMLO = interCrackValues.operatorHourlyCost * interCrackValues.operatorWeeklyTime / interCrackValues.operatorProductivityOthers;
            interCrackValues.materialCostBoardBalls = interCrackValues.numberOfComponents * interCrackValues.ballCostMLO;
            interCrackValues.materialCostBoardMLO = interconnectType == "MLO" && interconnectCost != "" ? Double.Parse(interconnectCost) : interCrackValues.mlo;
            interCrackValues.totalCostBoardMLO = interCrackValues.operatorCostBoardMLO + interCrackValues.materialCostBoardBalls + interCrackValues.materialCostBoardMLO;

            interCrackValues.operatorCostDirect = interCrackValues.operatorHourlyCost * interCrackValues.operatorWeeklyTime / interCrackValues.operatorProductivityOthers;
            interCrackValues.materialCostDirect = interconnectType == "Direct Attach" && interconnectCost != "" ? Double.Parse(interconnectCost) : interCrackValues.directAttach;
            interCrackValues.totalCostDirect = interCrackValues.operatorCostDirect + interCrackValues.materialCostDirect;

            interCrackValues.operatorCostInterposer = interCrackValues.operatorHourlyCost * interCrackValues.operatorWeeklyTime / interCrackValues.operatorProductivityOthers;
            interCrackValues.materialCostInterposer = interconnectType == "Interposer" && interconnectCost != "" ? Double.Parse(interconnectCost) : interCrackValues.interposer;
            interCrackValues.totalCostInterposer = interCrackValues.operatorCostInterposer + interCrackValues.materialCostInterposer;

            interCrackValues.operatorCostHybrid = interCrackValues.operatorHourlyCost * interCrackValues.operatorWeeklyTime / interCrackValues.operatorProductivityOthers;
            interCrackValues.materialCostHybrid = interconnectType == "Hybrid" && interconnectCost != "" ? Double.Parse(interconnectCost) : interCrackValues.hybrid;
            interCrackValues.totalCostInterposer = interCrackValues.operatorCostHybrid + interCrackValues.materialCostHybrid;

            interCrackValues.operatorCostSpace = interCrackValues.operatorHourlyCost * interCrackValues.operatorWeeklyTime / interCrackValues.operatorProductivityOthers;
            interCrackValues.materialCostSpace = interconnectType == "Space Transformer" && interconnectCost != "" ? Double.Parse(interconnectCost) : interCrackValues.spaceTransformer;
            interCrackValues.totalCostSpace = interCrackValues.operatorCostSpace + interCrackValues.materialCostSpace;

            interCrackValues.operatorCostPC = interCrackValues.operatorTime * interCrackValues.operatorHourlyCost;
            interCrackValues.machineCostPC = interCrackValues.operatorTime * interCrackValues.machineCost;
            interCrackValues.totalCostPC = interCrackValues.operatorCostPC + interCrackValues.machineCostPC;

            interCrackValues.totalMLO = interCrackValues.totalCostSoldering + interCrackValues.totalCostBoardMLO + interCrackValues.totalCostPC;
            interCrackValues.weeklyMLOCost = interCrackValues.weeklyMLOCracks * interCrackValues.totalMLO;
            interCrackValues.yearlyMLOCost = 52 * interCrackValues.weeklyMLOCost;

            interCrackValues.solderingModel = interCrackValues.totalCostSoldering;

            if (interconnectType == "MLO")
            {
                interCrackValues.boardModel = interCrackValues.totalCostBoardMLO + interCrackValues.totalCostSoldering;
            }else if (interconnectType == "Direct Attach")
            {
                interCrackValues.boardModel = interCrackValues.totalCostDirect;
            } else if (interconnectType == "Interposer")
            {
                interCrackValues.boardModel = interCrackValues.totalCostInterposer;
            } else if(interconnectType == "Hybrid")
            {
                interCrackValues.boardModel = interCrackValues.totalCostHybrid;
            } else {
                interCrackValues.boardModel = interCrackValues.totalCostSpace;
            }
            interCrackValues.pcModel = interCrackValues.boardModel + interCrackValues.totalCostPC;
        }
    }
}
