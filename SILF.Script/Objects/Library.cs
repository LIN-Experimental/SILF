﻿namespace SILF.Script.Objects;

public class Library
{


    public const string String = "string";
    public const string Number = "number";
    public const string LotNumber = "big";
    public const string Bool = "bool";
    public const string List = "array";
    public const string Null = "null";



    private readonly List<string> OtherTypes = [];



    /// <summary>
    /// Funciones y tipos.
    /// </summary>
    public Dictionary<Tipo, (List<IFunction>, List<IProperty>)> Objects { get; set; } = [];


    /// <summary>
    /// Cargar funciones a un tipo.
    /// </summary>
    /// <param name="type">Tipo.</param>
    /// <param name="functions">Lista de funciones.</param>
    public void Load(string type, List<IFunction> functions)
    {

        // Agregar nuevo tipo.
        OtherTypes.Add(type);

        // Si ya existe.
        if (Objects.TryGetValue(new(type), out var values))
        {
            values.Item1.AddRange(functions);
            return;
        }

        // Nuevo elemento.
        Objects.Add(new(type), (functions, []));

    }


    /// <summary>
    /// Cargar funciones a un tipo.
    /// </summary>
    /// <param name="type">Tipo.</param>
    /// <param name="properties">Lista de propiedades.</param>
    public void Load(string type, List<IProperty> properties)
    {

        // Si ya existe.
        if (Objects.TryGetValue(new(type), out var values))
        {
            values.Item2.AddRange(properties);
            return;
        }

        // Nuevo elemento.
        Objects.Add(new(type), ([], properties));

    }


    /// <summary>
    /// Obtener un nuevo objeto.
    /// </summary>
    /// <param name="type">Tipo</param>
    public SILFObjectBase Get(string type = Null)
    {

        // Normalizar.
        type = type.Trim();

        // Objeto final.
        SILFObjectBase obj;

        obj = new SILFClassObject(type)
        {
            Value = new
            {
            },
            Functions = GetFunctions(type).ToList(),
            Properties = GetProperties(type).ToList()
        };

        return obj;

    }


    /// <summary>
    /// Obtener los métodos.
    /// </summary>
    /// <param name="type">Tipo.</param>
    public IEnumerable<IFunction> GetFunctions(string type = Null)
    {
        // Tipo.
        type = type.Trim();

        // Data.
        var data = Objects.Where(t => t.Key.Description == type).Select(t => t.Value.Item1).SelectMany(t => t);

        // Retornar.
        return data ?? [];

    }


    /// <summary>
    /// Obtener las propiedades.
    /// </summary>
    /// <param name="type">Tipo.</param>
    public IEnumerable<IProperty> GetProperties(string type = Null)
    {

        // Tipo.
        type = type.Trim();

        // Data.
        var data = Objects.Where(t => t.Key.Description == type).Select(t => t.Value.Item2).SelectMany(t => t).Select(t => t.Clone());

        // Retornar.
        return data ?? [];

    }


    /// <summary>
    /// Obtener un nuevo objeto.
    /// </summary>
    /// <param name="type">Tipo</param>
    public Tipo? Exist(string type)
    {

        // Normalizar.
        type = type.Trim();

        // Tipos
        string[] localTypes = ["string", "number", "array", "big", "bool"];
        string[] types = [.. localTypes, .. OtherTypes];

        // Null.
        return types.Where(t => t == type).Select(t => new Tipo(t)).FirstOrDefault();

    }

}