->INIT

==INIT==
Liam: Hey
Player: Hi
Liam: I have a question. Which do you prefer? Music or Sports?
    *[I like music more.]
        ->MUSIC
    *[I like sports.]
        ->SPORTS
        
==SPORTS==
Liam: Oh, okay. I'm not much of a sports guy, to be honest.
Player: Gotcha.
->DONE

==MUSIC==
Liam: Sweet. Some of us are going to have a musical hangout if you want to join us.
Player: Okay. Sounds fun.
->DONE
