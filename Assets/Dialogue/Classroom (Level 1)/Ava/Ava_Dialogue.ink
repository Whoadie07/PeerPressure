->INIT

==INIT==
Ava: Hey, how's your day?
 *[Good]
    Ava: Me too. Execpt the homework is hard. ...Oh I cannot find my pencil. Do you have an additional one?
    ->DONE
 *[Awful]
    Ava: what happened? Is the homework bothering you?
    ->HOMEWORK
 
==HOMEWORK==
*[Yes]
    Ava: Me too. The homework is so confusing...By the way, did you see my pencil? I don't know where it's gone.
    ->DONE
*[No]
    Ava: I hope you're doing well...By the way, did you see my pencil? I don't know where it's gone.
    ->DONE