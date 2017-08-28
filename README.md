# Graywing

>   Queries facts in Prolog.

This repos contains Prolog scripts to which you can make queries about any cat's relations in _Warriors_ series, along with a .NET Core project that can generate the latest facts in Prolog from [Warriors Wiki](http://warriors.wikia.com/).

## To make queries

For now, the supported direct relations and deductive relations are limited. For a list of deductive relations available, see  [`/Prolog/rules.pl`](/Prolog/rules.pl)

You need to setup Prolog execution environment. For example, you may install [SWI-Prolog](http://www.swi-prolog.org/download/stable) for your Windows/Linux/MacOS.

Load [`/Prolog/main.pl`](/Prolog/main.pl), and make queries

```prolog
?- male('Firestar').        % Is Firestar male?
true.

?- male('Hollyleaf').       % Is Hollyleaf male?
false.

?- female('Mousefur').
true.

?- father(X, 'Bramblestar').            % Find Bramblestar's father
X = 'Tigerstar' .

?- child(X, 'Jake', 'Nutmeg (KP)').     % Find Jake and Nutmeg (KP)'s children
X = 'Firestar' ;                        % Press semicolon to ask for more results
X = 'Princess'.

?- findall(X, name(X, "Robinwing"), L).         % Find all the cats under the name Robinwing .
L = ['Robinwing (RC)', 'Robinwing (SC)', 'Robinwing (TC)', 'Robinwing (WC)'].

?- findall(X, child(X, 'Snowbird'), L).         % Find Snowbird's children 
L = ['Beenose', 'Berryheart', 'Bluebellkit', 'Cloverfoot', 'Conekit', 'Frondkit', 'Gullkit', 'Rippletail (SC)', 'Yarrowleaf'].

?- findall(X, grandchild(X, 'Firestar'), L).    % Find Firestar's grand-children
L = ['Alderheart', 'Dandelionkit', 'Hollyleaf', 'Jayfeather', 'Juniperkit', 'Lionblaze', 'Sparkpelt'].

?- findall(X, grandson(X, 'Firestar'), L).      % Find Firestar's grandsons
L = ['Alderheart', 'Jayfeather', 'Juniperkit', 'Lionblaze'].
```

