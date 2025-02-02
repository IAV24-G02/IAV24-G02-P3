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

C. El planteamiento del ejercicio es hacer una máquina de estados en la que se hallen distintas fases por las que los robots pasen en su comportamiento. En primer lugar habrá una fase de patrulla, donde el robot seguirá una serie de puntos de forma cíclica. Si se encontrase con `Nestor` (es decir, que entre en su cono de visión durante la patrulla), el robot pasará a una fase de seguimiento y disparo donde, mientras siga estando en dicho cono, seguirá siguiendo y disparando hasta acabar con él. Dentro del propio seguimiento hay momentos de recálculo del porcentaje de acierto y/o daño que se le puede hacer, pero siempre se vuelve rápidamente al estado de seguimiento y disparo. Pueden darse a partir de ahí dos casos distintos: o bien pierde a `Nestor` de vista y tiene que volver a la base a volver a realizar la patrulla o bien se queda sin munición y tiene que volver a la base para recargar. En éste último, el robot tiene que hacer todo el recorrido de vuelta independientemente de su posición actual. En el otro caso de pérdida de vista, el robot tendrá que localizar el punto de la patrulla más cercano y volver de nuevo a hacer la patrulla.

Todos estos estados se pueden ver bastante visualmente gracias al siguiente diagrama de estados:

```mermaid
flowchart TD
    A(PATROL) -->|Detect Nestor| B(FOLLOW_SHOOT)
    B --> |X frames passed| C(RECALCULATE_AIM)
    C -->  B
    D(RELOAD) --> |Bullets Reloaded & !Detect Nestor| A
    D --> |Bulltets Reloaded & Detect Nestor| B
    E(GO_TO_BASE) --> D
    B --> |No bullets| E

    F(GO_TO_NEAREST_WAYPOINT) --> A
    B --> |!Detect Nestor| F
```

Ian Millington en su su libro *AI for Games* habla sobre este tipo de situaciones en las que un personaje tiene que pasar por distintos _estados_ durante la partida, y muchas desarrolladoras cuando se les plantea también esta situación deciden optar por usar *máquinas de estados*. Esta opción tiene la ventaja de que queda a medio camino entre los árboles de comportamiento y los estados de los propios personajes. Sin entrar en mucho detalle de cómo de parte de una máquina de estados más esquemática a una más formal, como la que habrá que plantear de cara al apartado a resolver, la idea es partir de una estructurá más básica que permita la posterior modificación con los nuevos estados que se necesiten para la funcionalidad del robot. 

La máquina de estados mantiene un registro de las posibles transiciones y guarda el estado actual en él. Con cada estado, una serie de transiciones son mantenidas. Cada transición es, de nuevo, una interfaz genérica que puede ser implementada con las condiciones apropiadas. Simplemente notifica a la máquina de estados si debe ser activada o no.

```
class StateMachine:
    # We’re in one state at a time.
    initialState: State
    currentState: State = initialState

    # Checks and applies transitions, returning a list of actions.
    function update() -> Action[]:
        # Assume no transition is triggered.
        triggered: Transition = null

        # Check through each transition and store the first
        # one that triggers.
        for transition in currentState.getTransitions():
            if transition.isTriggered():
                triggered = transition
                break

        # Check if we have a transition to fire.
        if triggered:
            # Find the target state.
            targetState = triggered.getTargetState()

            # Add the exit action of the old state, the
            # transition action and the entry for the new state.
            actions = currentState.getExitActions()
            actions += triggered.getActions()
            actions += targetState.getEntryActions()

            # Complete the transition and return the action list.
            currentState = targetState
            return actions

        # Otherwise just return the current state’s actions.
        else:
            return currentState.getActions()
```

La máquina de estados cuenta con que tendrá estados y transiciones con una interzaz concreta. La interfaz de estado sigue la siguiente forma:


```
class State:
    function getActions() -> Action[]
    function getEntryActions() -> Action[]
    function getExitActions() -> Action[]
    function getTransitions() -> Transition[]
```

Cada uno de los métodos getXActions debería devolver una lista de acciones a realizar. Además, getentryActions solo es llamdo cuando el estado entra desde una transición, y getExitActions es solamente llamado cuando se sale de un estado. El resto del tiempo en el que el estado esté activo, se llama a getactions. El método getTransitions debería devolver una lista de transiciones que salen de este estado.

La interfaz de transición tien ela siguiente forma:

```
class Transition:
    function isTriggered() -> bool
    function getTargetState() -> State
    function getActions() -> Action[]
```

D. En este apartado se nos pide que con BehaviorBricks `Néstor` escape del **Nivel 2**. Para ello vamos a implementar el siguiente árbol de comportamientos:

```mermaid
flowchart TD
    A(("↺")) --> B((?))
    B -->|IsEnemyLooking| C[MoveToGameObject/MoveToNearestHidingSpot]
    B -->|CheckHealth| D[MoveToGameObject/MoveToNearestPossibleHealthPoint]
    B -->|IsActive/IsRedButtonActive| E[MoveToGameObject/MoveToRedButton]
    B -->|IsActive/IsBlueButtonActive| F[MoveToGameObject/MoveToBlueButton]
    B -->|IsActive/IsWhiteButtonActive| G[MoveToGameObject/MoveToWhiteButton]
    B -->|IsActive/IsGreenButtonActive| H[MoveToGameObject/MoveToGreenButton]
    B -->|IsActive/IsExitActive| I[MoveToGameObject/MoveToExit]
```

**Es importante conseguir todos los botones en ese orden para poder salir del nivel y, aunque para el caso de los botones rojo y azul puede variar el orden, conviene que primero sea el rojo y luego el azul para que sea el camino más rápido por la disposición de la escena.**

La idea es que siempre se compruebe si el enemigo está mirando a `Néstor` y si su vida es menor de cierto valor, en este caso, 25. Mientras no se cumpla ninguna de estas condiciones, `Néstor` debe intentar conseguir los botones que desbloquean las puertas. Si no hay botones activos, debe dirigirse a la salida porque ya ha conseguido todas las llaves y ha desbloqueado todas las puertas.

- Si el/los enemigo/s está/n mirando a `Néstor`, `Néstor` debe huir a un punto de escondite más cercano para perderlo/s de vista y volver a intentar conseguir los botones que desbloquean las puertas.

- Si la vida de `Néstor` es menor de cierto valor y dependiendo de si puede llegar hasta un punto de curación, `Néstor` irá al punto más cercano para curarse. Si ese punto más cercano no está disponible, `Néstor` seguirá intentando conseguir los botones que desbloquean las puertas.

Como se puede intuir, esto es un árbol de comportamientos *hardcodeado*, por lo que no es muy flexible y solo se puede aplicar a este caso en concreto.

E. En este último apartado modificaremos el árbol de comportamientos de `Néstor` para que sea más genérico y funcione para el **Nivel 2** y uno modificado por nosotros.

```mermaid
flowchart TD
    A(("↺")) --> B((?))
    B -->|IsEnemyLooking| C[MoveToGameObject/MoveToNearestHidingSpot]
    B -->|CheckHealth| D[MoveToGameObject/MoveToNearestPossibleHealthPoint]
    B -->|AlwaysTrue| E[SearchRooms]
```

```mermaid
flowchart TD
    A[SearchRooms] --> B
    B(("↺")) --> C((?))
    C -->|HasRegisteredExit| D[MoveToExit]
    C -->|HasRegisteredLockedDoorAndButton| E(("->"))
    E --> F[MoveToButton]
    E --> G[MoveToDoor]
    C -->|AlwaysTrue| H[NestorPatrol]
```

```mermaid
flowchart TD
    A[NestorPatrol] --> B
    B(("↺")) --> C(("->"))
    C --> D[ChooseNextWaypoint]
    C --> E[MoveToWaypoint]
    C --> F[TurnAroundAndRegister]
```

## Notas

- Para quitar zoom de la cámara, pulsar la tecla `Z`. Dejar de pulsarla para volver al zoom original.

- Cada vez que se realice el TurnAround, se registran: puertas bloqueadas, botones, la salida, puntos de salud, escondites y como el TurnAround se realiza en un WayPoint, establecemos que ese WayPoint ya está visitado.

- Cada vez que entre a una nueva sala o esté en ella se registran los WayPoints correspondientes a esa sala además de guardarse los últimos WayPoints sin visitar si existen por lo habría un registro de WayPoints relacionado con si está visitado o no.

- Se guardan los WayPoints sin visitar porque puede haber casos como que un guardia persiga a `Néstor` o que `Néstor` esté bajo de vida y necesite ir a un punto de salud o que haya desbloqueado una puerta.

- Cuando se han registrado puertas cerradas se comprueban con el registro de botones si coinciden en color. Si coinciden, `Néstor` iría al botón y secuencialmente a la puerta. Esto dejaría algunas salas sin visitar, pero como tenemos el registro de WayPoints, iría al primer WayPoint sin visitar.

- Cuando `Néstor` termina de visitar todos los waypoints de la sala en la que se encuentra actualmente o se ve interrumpida por otra tarea, se dirige al primer waypoint que dejó sin visitar y luego continúa visitando las salas sin visitar.

- Las tareas fallidas corresponden a la necesidad de buscar un waypoint para seguir patrullando.

- Las tareas completadas corresponden a las llegadas a un punto de salud, a un botón, a un escondite o a la salida y en el caso del apartado E. de haber realizado el TurnAround por completo en cada WayPoint y dejarlo visitado.

## Pruebas y métricas

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica A** | | |
| Comprobar la correcta representación del esquema con la topología del complejo militar incluyendo todos los elementos incluidos en este (Néstor, Cápsulas de Energía, botones, guardias y escondites) y el correcto zoom y seguimiento de la cámara a _Néstor_  | - Ejecución del programa y movimiento del jugador para inspeccionar el mapa<br> - Pulsaciones consecutivas de la tecla Z para observar el correcto funcionamiento del mismo. | [ZOOM OUT](https://drive.google.com/file/d/1tgHym8qujXc4JhgzAzM_VnA-O8f_rXHc/view?usp=sharing) |

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica B** | | |
| Probar el correcto bloqueo de la visibilidad de _Néstor_ al encontrarse tras una puerte cerrada o abierta. | - Colocarse tras una puerta cerrada. <br> - Colocarse tras una puerta abierta. | [PUERTAS](https://drive.google.com/file/d/1xDdn_6VpPXWPO8Dl23JN1PwGeRgeO7Vm/view?usp=sharing) |
| Probar el correcto bloqueo de la visibilidad de _Néstor_ al encontrarse en el interior de un montón de robots. | - Colocarse en el interior de un montón de robots mientras _Néstor_ se encuentra en el cono de visión de un robot. | [MONTONES](https://drive.google.com/file/d/1a8Bazp3TvsrvPPAMe_1Jk2yMdbPS4PYI/view?usp=sharing) |

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica C** | | |
| Probar que el comportamiento de los robot es el descrito en la máquina de estados asignada a los mismos. | - Entrar en el cono de visión de un robot y observar que te sigue. <br> - Alejarte y acercarte y observar el empeoramiento/mejoría de la puntería. <br> - Aguantar los disparos necesarios para que se agoten y observar que vuelve a su base para recargar. <br> - Desaparecer del cono de visión del robot y observar que vuelve al Waypoint más cercano.  | [COMPORTAMIENTO GUARDIAS](https://drive.google.com/file/d/1c3Ql9Tgz8TNecZbubc4WdTEDZ5wJB5ul/view?usp=sharing) |
|  | - <br> - | []() |

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica D** | | |
| `Néstor` va hacia la energía cuando tiene poca vida y supera el nivel de ejemplo | - Salud < 25 <br> - | [PRIMER NIVEL](https://drive.google.com/file/d/1C3Z2MO5HSn2laQ6Oe0Ervxf8qnRsIIkZ/view?usp=sharing) |

| Pruebas | Métricas | Links |
|:-:|:-:|:-:|
| **Característica E** | | |
| `Néstor` supera el nivel de ejemplo sin guardias | - <br> - | [SEGUNDO NIVEL](https://drive.google.com/file/d/1tqpZNfKwkoWwuPY-zqf9YeTGcak3bTSC/view?usp=sharing) |
| `Néstor` supera el nivel modificado sin guardias | - <br> - | [TERCER NIVEL](https://drive.google.com/file/d/1JZDvm8CuM8yW4AeAetEV7sigTUYyDbLm/view?usp=sharing) |

[ENLACE AL VÍDEO COMPLETO EN YOUTUBE](https://youtu.be/Hyjrv2IvgoE)  
[ENLACE AL VÍDEO COMPLETO EN DRIVE](https://drive.google.com/file/d/1UiNMjgLb2_Ug4UxNS5yX1aKOT3B9pJCC/view?usp=sharing)

## Ampliaciones

Se han realizado las siguiente ampliaciones:
- La mejora de la gestión sensorial. Se ha aplicado el sensor que tienen los guardias a `Néstor` para que pueda detectar los diferentes elementos de la escena. Estos elementos pueden ser vistos en el inspector de Unity en el componente `Register.cs` de `RobotKyle`.

## Producción

Las tareas se han realizado y el esfuerzo ha sido repartido entre los autores. Observa la tabla de abajo para ver el estado y las fechas de realización de las mismas. Puedes visitar nuestro proyecto de GitHub en el siguiente [link](https://github.com/orgs/IAV24-G02/projects/3/).

| Estado  |  Tarea  |  Fecha  |  
|:-:|:--|:-:|
| ✔️ | Diseño: Primer borrador | 18-04-2024 |
| ✔️ | Característica A | 10-04-2024 |
| ✔️ | Característica B | 10-04-2024 |
| ✔️ | Característica C | 29-04-2024 |
| ✔️ | Característica D | 23-04-2024 |
| ✔️ | Característica E | 25-04-2024 |
|  |  **OTROS**  | |
| ✔️ | Recorrido inteligente de `Néstor` de forma que visita aquello que ha dejado sin visitar cuando no encuentra un botón del mismo color que las puertas vistas. | 25-04-2024 |
| ✔️ | Recorrido por waypoints y realización de un turn-around para registrar los objetos visionados por el sensor. | 25-04-2024 |
| ✔️ | Durante el recorrido, si debe recuperar energía/salud, va al punto de salud y vuelve al último punto donde se encontraba. | 25-04-2024 |
|  |  **OPCIONALES**  | |
| ✔️ | Mejora de la gestión sensorial, de manera que el protagonista vea a los personajes no necesariamente compartiendo estancia, razonando también sobre el conocimiento que le aportan esas percepciones, y mostrando realimentación visual para que quede claro lo que ha visto. | 25-04-2024 |

## Licencia

Yi (Laura) Wang Qiu, Agustín Castro De Troya, Ignacio Ligero Martín, Alfonso Jaime Rodulfo Guío, autores de la documentación, código y recursos de este trabajo, concedemos permiso permanente a los profesores de la Facultad de Informática de la Universidad Complutense de Madrid para utilizar nuestro material, con sus comentarios y evaluaciones, con fines educativos o de investigación; ya sea para obtener datos agregados de forma anónima como para utilizarlo total o parcialmente reconociendo expresamente nuestra autoría.

Una vez superada con éxito la asignatura se prevee publicar todo en abierto (la documentación con licencia Creative Commons Attribution 4.0 International (CC BY 4.0) y el código con licencia GNU Lesser General Public License 3.0).

## Referencias

Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington.
- [Liquid Snake](https://ceur-ws.org/Vol-3305/paper7.pdf)
- Behavior Bricks de PadaOne Games (empresa de base tecnológica de la UCM)
https://assetstore.unity.com/packages/tools/visual-scripting/behavior-bricks-74816
- Unity, Navegación y Búsqueda de caminos
https://docs.unity3d.com/es/2021.1/Manual/Navigation.html
- Unity 2018 Artificial Intelligence Cookbook, Second Edition (Repositorio)
https://github.com/PacktPublishing/Unity-2018-Artificial-Intelligence-Cookbook-Second-Edition 
- Unity Artificial Intelligence Programming, 5th Edition (Repositorio)
PacktPublishing/Unity-Artificial-Intelligence-Programming-Fifth-Edition: Unity Artificial Intelligence Programming – Fifth Edition, published by Packt (github.com)
