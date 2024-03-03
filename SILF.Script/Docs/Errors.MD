# Códigos de error


Con SILF Script 2024.3 se agregaron los códigos de error para facilitar el control de errores.



### SC001
La función no tiene los parámetros necesarios.

Ocurre cuando tratamos de ejecutar una función con la cantidad errónea de parámetros.


```java
Saludar() 

function void Saludar(string name)
   print($"Hola {name}")
```

Falta el parámetro ```name```


### SC002
Un parámetro de una función está recibiendo un valor del tipo incorrecto.

```java
Saludar(22) 

function void Saludar(string name)
   print($"Hola {name}")
```

Falta el parámetro ```name``` esta recibiendo un valor del tipo ```number``` pero esta esperando un valor del tipo ```string```


### SC003
Error al realizar operaciones.

Por lo general sucede cuando no se termino de escribir la expresión completa.

```java
number suma = 1 + 
```


### SC004
El operador no es compatible con los tipos ```x``` y ```z```

```java
let suma = 22 + false
```


### SC005 
Error de conversión numérica.

### SC006
Nombre invalido

### SC007
No se encontró el tipo <x>

### SC008
Las variables declaradas con LET se deben asignar.

### SC009
Tener un valor valido.

### SC010
El valor esperado no puede ser vacío <void>

### SC011
El tipo <x> no puede ser convertido en <y>

### SC012
El tipo <x> no esta definido

### SC013
Ya existe un elemento con este nombre

### SC014
El valor de una constante no puede ser ‘mutable

### SC015
Parámetro sin nombre en una función

### SC016
No se encontró el método main de la aplicación

### SC017
No existe el elemento <x>

### SC018
No se puede modificar el valor de X ya que es de solo lectura.

### SC019
No existe el método/propiedad dentro del elemento <x>

### SC020
El campo <x> no ha sido asignado.

### SC021
Expresión invalida.

### SC022
Una función marcada como void no puede regresar nada.

### SC023
El tipo ```X``` no tiene la propiedad de indices.