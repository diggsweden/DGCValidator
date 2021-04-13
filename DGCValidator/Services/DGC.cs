using System;
namespace DGCValidator.Services
{
    /**
    * A DGC representation class.
    *
    * @author Henrik Bengtsson (henrik@sondaica.se)
    * @author Martin Lindström (martin@idsec.se)
    * @author Henric Norlander (extern.henric.norlander@digg.se)
    */
    public class DGC
    {
        public Vproof vProof { get; set; }
        public String issuingCountry { get; set; }
        public DateTime expirationDate { get; set; }
        public DateTime issuedDate { get; set; }

        public DGC()
        {
            vProof = new Vproof();
            vProof.Sub = new Sub();
        }
    }
}
