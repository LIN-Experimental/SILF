﻿using System.Text.Json;

namespace SILF.Script.Elements;


internal class Field : IEstablish
{

    /// <summary>
    /// Nombre de la variable
    /// </summary>
    public string Name { get; set; } = string.Empty;


    /// <summary>
    /// Si el elemento fue asignado
    /// </summary>
    public bool IsAssigned { get; set; }




    /// <summary>
    /// Valor de la variable
    /// </summary>
    public Objects.SILFObjectBase? Value { get; set; }


    /// <summary>
    /// Tipo de la variable
    /// </summary>
    public Tipo? Tipo { get; set; }


    /// <summary>
    /// Nivel de asolación
    /// </summary>
    public Isolation Isolation { get; set; }


    public Instance Instance { get; set; }



    /// <summary>
    /// Constructor
    /// </summary>
    public Field(string name, Objects.SILFObjectBase value, Tipo tipo, Instance instance, Isolation isolation = Isolation.ReadAndWrite)
    {
        this.Name = name;
        this.Value = (value);
        this.Tipo = tipo;
        this.Isolation = isolation;
        this.Instance = instance;
    }


    /// <summary>
    /// Constructor
    /// </summary>
    public Field()
    {
    }



    /// <summary>
    /// Obtener el tamaño en bytes del campo.
    /// </summary>
    public int GetInt()
    {

        if (Instance.Environment == Environments.PreRun)
            return 0;


        // Serializar el objeto a JSON
        string json = JsonSerializer.Serialize(Value);

        // Calcular el tamaño del JSON en bytes
        int total = Encoding.UTF8.GetBytes(json).Length;


        return total;
    }



    public void Establish(SILFObjectBase objectBase)
    {

        if (Isolation != Isolation.ReadAndWrite & Isolation != Isolation.Write)
        {
            Instance.WriteError("SC018", $"El campo '{Name}' no se puede se puede sobrescribir.");
        }


        Value = objectBase;
        IsAssigned = true;
    }
}