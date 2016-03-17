using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RequestLibrary;

namespace PreparationRoom
{

    public enum somethingElse { Talvez, Sim, Nao };

    public class PreparationRoom
    {
        public PreparationRoomID Room { get; private set; }
        private List<RequestLine> rls;


        public PreparationRoom(PreparationRoomID Room)
        {
            this.Room = Room;
        }
    }
}
