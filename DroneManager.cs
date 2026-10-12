using Lab_4_OOP;
using System;
using System.Collections.Generic;

namespace Lab_4_OOP
{
    public class DroneManager
    {
        private List<Drone> droneList = new List<Drone>();
        private int currentId = 1;

        public int GetNextId()
        {
            return currentId++;
        }

        public void AddDroneDirectly(Drone drone)
        {
            if (drone.Id <= 0)
            {
                drone.SetAssignedId(GetNextId());
            }
            droneList.Add(drone);
        }

        public List<Drone> GetList()
        {
            return droneList;
        }

        public Drone FindById(int id)
        {
            for (int i = 0; i < droneList.Count; i++)
            {
                if (droneList[i].Id == id) return droneList[i];
            }
            return null;
        }

        public bool RemoveDrone(int id)
        {
            Drone target = FindById(id);
            if (target != null)
            {
                droneList.Remove(target);
                return true;
            }
            return false;
        }
    }
}
