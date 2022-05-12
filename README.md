![UCU](https://github.com/ucudal/PII_Conceptos_De_POO/raw/master/Assets/logo-ucu.png)

### FIT - Universidad Católica del Uruguay

## Ejercicio Herencia

### Objetivo
El objetivo de este ejercicio es el de aplicar los conceptos de herencia y agregación, así como también
sobrecarga y sobreescritura de métodos y clases abstractas.

### Introducción
Últimamente, se ha vuelto muy difícil encontrar lugar para estacionar en la cercanía de la universidad. Es por ello, que la facultad de Ingeniería, ha tomado la iniciativa de crear un programa de "Ridesharing" gratuito entre profesores y alumnos de la universidad. 
Este nuevo sistema, denominado UCURide funcionará de la siguiente forma: 
Tanto alumnos, como profesores podrán postularse como voluntarios, ofreciendo sus vehículos como medio compartido. A estos se los denomnará **Conductores**. Todos los demas usuarios seran los **Pasajeros**.
 * Los Conductores tendran ciertos atributos mínimos, como ser Nombre, Apellido, Cédula, un Vehículo<sup>1</sup> Calificación como conductor y una breve bio.
 * Los Pasajeros tendran sus propios atributos, como ser Cédula, Nombre, Apellido, calificación como Pasajero.<sup>2</sup>

A su vez, los conductores podran ser identificados de dos formas: 
* **Comunes:** Solamente levaran a un pasajero por vez
* **Pool:** Estan dispuestos a realizar un viaje con multiples pasajeros. Para ello, es necesario saber la capacidad máxima de pasajeros que puede llevar.

Con el fin de promocionar este nuevo servicio, la Facultad ha creado una nueva cuenta de Twitter, en la cual desea publicar cada nuevo registro de conductores y pasajeros. Cada vez que un nuevo voluntario se registra como conductor, se publicará una foto del conductor con su bio y un mensaje de bienvenida. Cada vez que un nuevo pasajero se registra para utilizar la app se debera publicar tambien su foto<sup>3</sup> de perfil con su nombre.


### Desafío 1
Deberás identificar todas las clases del problema y como estas se relacionan entre si. Para representar esto, debes utilizar un diagrama de clases muy básico, similar a los visto en los ejemplos. Podrás realizar esto en papel y subir una foto del mismo.

### Desafío 2
Deberas programar todas estas clases junto con un ejemplo de ejecución en el Program. 
En cuanto al funcionamiento del sistema, solamente es necesario mantener una lista de **Conductores** y **Pasajeros** y realizar la publicación correspondiente cuando se agrega un nuevo pasajero o conductor. Para ello, podrás hacer uso de la API de twitter provista en el siguiente repo: https://github.com/ucudal/PII_TwitterApi

### Desafío 3 (Desafío Bonus!)
Si bien la universidad espera que los usuarios, al ser adultos, no publiquen fotos inadecuadas en twitter, todos pueden cometer errores y por lo tanto se desea aplicar un filtro a las potenciales fotos a publicar:
 * Todas las fotos de perfil de los pasajeros deberan contener una cara.
 * Todas las fotos de perfil de los conductores deberan contener una cara y además estar sonriendo. 

Para ello, podrás hacer uso de la API de twitter provista en el siguiente repo: https://github.com/ucudal/PII_CognitiveAPI

****

<sup>1</sup>*A los efectos de este sistema, no es necesario diferenciar entre Profesores y Alumnos*</br>
<sup>2</sup>*No es necesario modelar la clase Vehículo, pero puedes hacerlo si te divierte*</br>
<sup>3</sup>*Solamente un string con el path a la imagen es suficiente para este ejercicio*
