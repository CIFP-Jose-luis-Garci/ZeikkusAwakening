using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLookupTable
{
    public static string DialogueLookup(int identifier)
    {
        switch (identifier)
        {
            case 0:
                GameManager.talking = "???";
                return "Mi futuro... mi vida al completo... ¡todo me lo arrebataste tú! Pero no te preocupes... el destino que te aguarda está a mi merced.";
            case 1:
                GameManager.currentEvent = 1;
                return "Es el momento. Padre ya no puede protegerte. Y él está dormido. A través de tí, conseguiré mis deseos, Zeikku. Gracias a este poder que recibí... es hora de llevar a cabo mi venganza.";
            case 2:
                GameManager.talking = "???";
                return "Zeikku...  Zeikku, ¡despierta! ¡Despierta de tu sueño!";
            case 3:
                GameManager.talking = "Zeikku";
                return "... ¿Eh? ¿Dónde... dónde estoy?";
            case 4:
                GameManager.talking = "Lailah";
                return "Soy yo, Lailah.";
            case 5:
                GameManager.talking = "Zeikku";
                return "¡¿Lailah?! ¿Lailah Ernest?";
            case 6:
                GameManager.talking = "Lailah";
                return "Me conocías con ese nombre, pero ya no estoy más en el mundo real. Soy una cognición de tu mente, una simple ilusión creada por tí. ";
            case 7:
                return "Escúchame, tienes que escapar de la prisión.";
            case 8:
                GameManager.talking = "Zeikku";
                return "Espera, ¿cómo? ¿Qué prisión? ¿Dónde estoy exactamente?";
            case 9:
                GameManager.talking = "Lailah";
                return "Estás en el interior de tu mente. Eres lo poco que queda de tí dentro de la mente corrompida por tu hermana.";
            case 10:
                return "Hace 10 años, cuando te coronaron, tu hermana se apoderó de tu cuerpo con una maldición que ha impedido tu consciencia durante todo este tiempo.";
            case 11:
                return "Desde entonces, ha estado cometiendo diferentes atrocidades y problemas a la gente tanto de Hellgart como de Delvill, y por si eso no fuera suficiente se ha embarcado en la búsqueda del Ojo de Apolo para dominar el mundo.";
            case 12:
                GameManager.talking = "Zeikku";
                return "Zinnia... ¿eras tú? Maldita sea..";
            case 13:
                GameManager.talking = "Lailah";
                return "Llevo 10 años esperando que despiertes para poder derrotarla, pues sólo tú tienes el poder del Dios del Espacio. Tu padre, quién había forjado un lazo estrecho con su encarnación, no lo sabía, pero él te eligió como recipiente donde estaría a salvo en busca de los restos del mal.";
            case 14:
                GameManager.currentEvent = 2;
                return "Invocalo. Haz que venga junto a tí ese poder que una vez nos salvó a todos.";
            case 15:
                GameManager.talking = "Zeikku";
                return "Este... ¿es el poder?";
            case 16:
                GameManager.talking = "Lailah";
                return "La espada legendaria Zagrant, sí. Es la forma que una vez derrotado, el dios tomó para volverse inmortal. Tú eres su única esperanza para poder destruir los resquicios de la oscuridad que acechan tu mundo.";
            case 17:
                return "Te espero a la salida de la prisión. Hasta entonces, ten cuidado, pues innumerables peligros te aguardan en el camino.";
            case 18:
                GameManager.talking = "Zeikku";
                GameManager.currentEvent = 3;
                return "Lailah... De acuerdo, ¡espérame!";
            case 19:
                GameManager.talking = "???";
                return "¡Fuera de aquí! ¡¿No véis que ya no os queda nada más por destruir?!";
            case 20:
                return "Juro venganza sobre el rey que destruyó mi mundo y asesinó a mis padres. Mist nos ayudará si la convencemos, estoy seguro, sólo tiene que entender que no podemos quedarnos de brazos cruzados.";
            case 21:
                GameManager.currentEvent = 4;
                return "Nosotros, ¿héroes, no? No podría imaginarlo hace unos años...";
            default:
                return "";
        }
    }
}
