using System.Collections.Generic;
using UnityEngine;

namespace Building
{
    public struct BuilidngPositionsPair
    {
        public BuilidngPositionsPair(int BuildingHeight, int BuildingWidth, Building Building)
        {
            this.Building = Building;
            CellsThisBuildingIsBuiltOn = new(BuildingWidth * BuildingHeight);
        }
        
        public Building Building;
        public List<Vector3Int> CellsThisBuildingIsBuiltOn;
    }
}