﻿namespace SILF.Script.Interfaces;

public interface IProperty : IEstablish
{

    /// <summary>
    /// Nombre de la propiedad.
    /// </summary>
    public string Name { get; set; }


    /// <summary>
    /// Valor.
    /// </summary>
    public Objects.SILFObjectBase Value { get; set; }


    /// <summary>
    /// Objeto base.
    /// </summary>
    public SILFObjectBase Parent { get; set; }


    /// <summary>
    /// Al obtener.
    /// </summary>
    public IFunction Get { get; set; }


    /// <summary>
    /// Al establecer.
    /// </summary>
    public IFunction Set { get; set; }


    /// <summary>
    /// Obtener el valor.
    /// </summary>
    /// <param name="instance">Instancia.</param>
    public SILFObjectBase GetValue(Instance instance);


    /// <summary>
    /// Establecer el valor.
    /// </summary>
    /// <param name="instance">Instancia.</param>
    /// <param name="base">Base.</param>
    public void SetValue(Instance instance, SILFObjectBase @base);


    /// <summary>
    /// Clonar la propiedad.
    /// </summary>
    public IProperty Clone();
}