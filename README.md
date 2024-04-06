# IAV - Práctica 3: Decisión

## Autores
- Yi (Laura) Wang Qiu [GitHub](https://github.com/LauraWangQiu)
- Agustín Castro De Troya [GitHub](https://github.com/AgusCDT)
- Ignacio Ligero Martín [GitHub](https://github.com/theligero)
- Alfonso Jaime Rodulfo Guío [GitHub](https://github.com/ARodulfo)

## Propuesta
Este proyecto es una práctica de la asignatura de Inteligencia Artificial para Videojuegos del Grado en Desarrollo de Videojuegos de la UCM, cuyo enunciado original es este: [Robot a la Fuga](https://narratech.com/es/inteligencia-artificial-para-videojuegos/decision/robot-a-la-fuga/).

[Descripción] ...

## Punto de partida

Se parte de un proyecto base de **Unity 2022.3.5f1** proporcionado por el profesor y disponible en este repositorio: [IAV-Robot](https://github.com/Narratech/IAV-Robot)

| Clases | Información |
| - | - |
| BasicRigidBodyPush | |
| BehaviourExecutor | |
| Bullet | Componente utilizado para los proyectiles. Gestiona la colisión de estos con otros objetos. Si colisiona con un objeto con el componente `Health` le resta vida. En cualquier caso los proyectiles se destruyen al colisionar o pasados 2 segundos desde que se intanciaron. |
| Enemy | Inicializa los atributos y comportamientos comunes a los Guardias: navegación por el entorno, árboles de comportamiento, salud y animaciones. |
| Health | |
| LaserShooter | Gestiona el disparo de los enemigos. Contiene variables para las balas, velocidad, cooldown y punto al que disparar. En su método `Shoot()` se instancian las balas desde el punto de disparo hacia un punto especificado o hacia delante. |
| PlayerInput | |
| ResetPlayer | |
| StarterAssetsInputs | |
| ThirdPersonController | |
| ToggleBarrier | |
| Waypoint | |

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

D. ...

```pseudo

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
|  | Característica A | ..-04-2024 |
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
