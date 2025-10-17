Hello I am testing choice implentation in ink.
Do you want to particpate?
*[yes]
    ->multipleChoice
*[no]
    Too bad
    ->multipleChoice

== multipleChoice ==
What is your favorite color?
*[Blue]
    ->checkIFButtonsGone
*[Red]
    ->DONE
*[Green]
    wrong
    ->DONE
*[Purple]
    ->DONE

== checkIFButtonsGone ==
Are the buttons gone?
*[yes]
    ->DONE
*[no]
    Go fix it
    ->DONE