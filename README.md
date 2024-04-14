# IAV - Práctica 3: Decisión

## Autores
- Yi (Laura) Wang Qiu [GitHub](https://github.com/LauraWangQiu)
- Agustín Castro De Troya [GitHub](https://github.com/AgusCDT)
- Ignacio Ligero Martín [GitHub](https://github.com/theligero)
- Alfonso Jaime Rodulfo Guío [GitHub](https://github.com/ARodulfo)

## Propuesta
Este proyecto es una práctica de la asignatura de Inteligencia Artificial para Videojuegos del Grado en Desarrollo de Videojuegos de la UCM, cuyo enunciado original es este: [Robot a la Fuga](https://narratech.com/es/inteligencia-artificial-para-videojuegos/decision/robot-a-la-fuga/).

Esta práctica consiste en recrear un escenario en el que un robot `Néstor` debe encontrar la salida del espacio en donde se encuentra. Para ello, deberá buscar llaves para desbloquear puertas y, huir y esconderse entre otros `Robots` de un grupo de `Guardias` que intentan capturarlo y disparlo si está en sus campos de visión. Estos últimos, si no ven a `Néstor`, patrullan por el escenario siguiendo waypoints.

## Punto de partida

Se parte de un proyecto base de **Unity 2022.3.5f1** proporcionado por el profesor y disponible en este repositorio: [IAV-Robot](https://github.com/Narratech/IAV-Robot)

| Clases | Información |
| - | - |
| BasicRigidBodyPush | Clase que controla el empuje de un RigidBody. Se asigna una fuerza con un rango de entre 0,5 y 5 (empezando con valor de 1,1) y, si se activa, verifica primero que no se colisione con otro RigidBody cinemático, que se choque con la capa deseada y que no se colisione con objetos que se encuentren por debajo para a continuación calcular la dirección del empuje y aplicar la fuerza al receptor. |
| BehaviorExecutor | Definición del componente BehaviorExecutor dentro de Unity. Incluye métodos de comportamiento que se actualizan desde el editor, con funcionalidad adicionales para el modo de depuración. |
| Bullet | Componente utilizado para los proyectiles. Gestiona la colisión de estos con otros objetos. Si colisiona con un objeto con el componente `Health` le resta vida. En cualquier caso los proyectiles se destruyen al colisionar o pasados 2 segundos desde que se intanciaron. |
| Enemy | Inicializa los atributos y comportamientos comunes a los Guardias: navegación por el entorno, árboles de comportamiento, salud y animaciones. |
| Health | Componente de vida del jugador. Tiene variables para la vida actual y la vida máxima con métodos para devolver el valor mínimo, máximo y el actual. Además, permite aplicar daño o resetear la vida de éste. |
| LaserShooter | Gestiona el disparo de los enemigos. Contiene variables para las balas, velocidad, cooldown y punto al que disparar. En su método `Shoot()` se instancian las balas desde el punto de disparo hacia un punto especificado o hacia delante. |
| PlayerInput | Esquema que mapea las acciones del jugador con botones de teclado, ratón y gamepad. |
| ResetPlayer | Reestablece la posición y rotación del jugador y de la cámara. |
| StarterAssetsInputs | Establece los valores de input: move, look, jump, sprint, crouch, shoot, interact que representan el estado del jugador en escena. |
| ThirdPersonController | Recoge todos los apartados necesarios para el satisfactorio control de una entidad en tercera persona. Realiza las labores de: salto y caída con representación de la gravedad determinada, comprobación de que la entidad se encuentra con los pies en la tierra, comprobación de que la entidad puede dejar de estar agachada y mueve la entidad en una dirección establecida a través de la rotación y la velocidad de la entidad. |
| ToggleBarrier | Corrutina que se encarga de cambiar el parámetro alpha de los mesh de los láseres de las barreras durante un tiempo predeterminado para representar su activación o desactivación. |
| Waypoint | Clase base que sive como almacenamiento para un punto de ruta que seguirá una entidad guiada por IA. |

## Diseño de la solución

A. ...

```pseudo

```

B. ...

```pseudo

```

C. ...

```pseudo

```

D. En este apartado se nos pide que con BehaviorBricks Nestor escape del Nivel 2. Para ello vamos a implementar el siguiente árbol de comportamientos:

```mermaid
flowchart TD
    A[Repeat] --> B((?))
    B --> C((?))
    C -->|Not caught && health < 250| D[MoveToHealthPoint]
    C -->|Not caught && health < 250| E[MoveToHealthPoint]
    C -->|Not caught && health < 250| F[MoveToHealthPoint]
    B -->|Not caught| G[MoveToRedButton]
    B -->|Not caught| H[MoveToBlueButton]
    B -->|Not caught| I[MoveToWhiteButton]
    I -->|Not caught| K[MoveToGreenButton]
    K --> L[MoveToExit]
```


E. ...

```pseudo

```

## Pruebas y métricas

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica A** | | |
|  | - <br> - | []() |
|  | - <br> - | []() |

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica B** | | |
|  | - <br> - | []() |
|  | - <br> - | []() |

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica C** | | |
|  | - <br> - | []() |
|  | - <br> - | []() |

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica D** | | |
|  | - <br> - | []() |
|  | - <br> - | []() |

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica E** | | |
|  | - <br> - | []() |
|  | - <br> - | []() |

[ENLACE AL VÍDEO COMPLETO EN YOUTUBE]()
[ENLACE AL VÍDEO COMPLETO EN DRIVE]()

## Ampliaciones

No se han realizado ampliaciones.

## Producción

Las tareas se han realizado y el esfuerzo ha sido repartido entre los autores. Observa la tabla de abajo para ver el estado y las fechas de realización de las mismas. Puedes visitar nuestro proyecto de GitHub en el siguiente [link](https://github.com/orgs/IAV24-G02/projects/3/).

| Estado  |  Tarea  |  Fecha  |  
|:-:|:--|:-:|
|  | Diseño: Primer borrador | ..-04-2024 |
|  | Característica A | 10-04-2024 |
|  | Característica B | ..-04-2024 |
|  | Característica C | ..-04-2024 |
|  | Característica D | ..-04-2024 |
|  | Característica E | ..-04-2024 |
|  |  **OTROS**  | |
|  |  | ..-04-2024 |
|  |  **OPCIONALES**  | |
|  |  | ..-04-2024 |

## Licencia

Yi (Laura) Wang Qiu, Agustín Castro De Troya, Ignacio Ligero Martín, Alfonso Jaime Rodulfo Guío, autores de la documentación, código y recursos de este trabajo, concedemos permiso permanente a los profesores de la Facultad de Informática de la Universidad Complutense de Madrid para utilizar nuestro material, con sus comentarios y evaluaciones, con fines educativos o de investigación; ya sea para obtener datos agregados de forma anónima como para utilizarlo total o parcialmente reconociendo expresamente nuestra autoría.

Una vez superada con éxito la asignatura se prevee publicar todo en abierto (la documentación con licencia Creative Commons Attribution 4.0 International (CC BY 4.0) y el código con licencia GNU Lesser General Public License 3.0).

## Referencias

Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington.
- [Liquid Snake](https://ceur-ws.org/Vol-3305/paper7.pdf)
- Unity Asset Store: [Behavior Bricks](https://assetstore.unity.com/packages/tools/visual-scripting/behavior-bricks-74816)
