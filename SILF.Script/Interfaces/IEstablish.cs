namespace SILF.Script.Interfaces;


public interface IEstablish
{


    Tipo? Tipo { get; set; }

    public void Establish(SILFObjectBase obj);


}