using System.Collections.Generic;
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
