# Gray Wing

>   Queries facts in Prolog.

This repos contains Prolog scripts to which you can make queries about any cat's relations in _Warriors_ series, along with a .NET Core project that can generate the latest facts in Prolog from [Warriors Wiki](http://warriors.wikia.com/).

## To make queries

For now, the supported direct relations and deductive relations are limited. For a list of deductive relations available, see  [`/Prolog/rules.pl`](/Prolog/rules.pl)

You need to setup Prolog execution environment. For example, you may install [SWI-Prolog](http://www.swi-prolog.org/download/stable) for your Windows/Linux/MacOS.

Load [`/Prolog/main.pl`](/Prolog/main.pl), and make queries

```prolog
?- male('Firestar').                % Is Firestar male?
true.

?- male('Hollyleaf').               % Is Hollyleaf male?
false.

?- female('Mousefur').
true.

?- father(X, 'Bramblestar').                    % Find Bramblestar's father
X = 'Tigerstar' .

?- child(X, 'Jake', 'Nutmeg (KP)').             % Find Jake and Nutmeg (KP)'s children
X = 'Firestar' ;                                % Press semicolon to ask for more results
X = 'Princess'.

?- findall(X, name(X, "Robinwing"), L).         % Find all the cats under the name Robinwing .
L = ['Robinwing (RC)', 'Robinwing (SC)', 'Robinwing (TC)', 'Robinwing (WC)'].

?- findall(X, child(X, 'Snowbird'), L).         % Find Snowbird's children 
L = ['Beenose', 'Berryheart', 'Bluebellkit', 'Cloverfoot', 'Conekit', 'Frondkit', 'Gullkit', 'Rippletail (SC)', 'Yarrowleaf'].

?- findall(X, grandchild(X, 'Firestar'), L).    % Find Firestar's grand-children
L = ['Alderheart', 'Dandelionkit', 'Hollyleaf', 'Jayfeather', 'Juniperkit', 'Lionblaze', 'Sparkpelt'].

?- findall(X, grandson(X, 'Firestar'), L).      % Find Firestar's grandsons
L = ['Alderheart', 'Jayfeather', 'Juniperkit', 'Lionblaze'].

?- findall(X, apprentice(X, 'Firestar'), L).    % Find Firestar's apprentices
L = ['Brackenfur', 'Bramblestar', 'Cherrytail', 'Cinderpelt', 'Cloudtail'].

?- findall(X, (apprentice(Y, 'Firestar'), apprentice(X, Y)), L); true.  % Find Firestar's apprentices' apprentice (2nd. order apprentices)
L = ['Hollyleaf', 'Icecloud', 'Sorrelstripe', 'Tawnypelt', 'Tigerheart', 'Whitewing', 'Berrynose', 'Rockshade', 'Leafpool'|...] [write]
L = ['Hollyleaf', 'Icecloud', 'Sorrelstripe', 'Tawnypelt', 'Tigerheart', 'Whitewing', 'Berrynose', 'Rockshade', 'Leafpool', 'Brightheart', 'Cherryfall', 'Cinderheart', 'Flametail', 'Hollytuft', 'Rainwhisker', 'Toadstep']

?- belongsto('Firestar', X).                    % Find Firestar's current clan
X = 'StarClan'.

?- belongsto(X, 'The Kin').                     % Find the members of Darktail's Kin
X = 'Cloverfoot' ;
X = 'Darktail' ;
X = 'Grassheart' ;
X = 'Max (Ro)' ;
X = 'Needletail' ;
X = 'Nettle (VS)' ;
X = 'Pinenose' ;
X = 'Rain (TAQ)' ;
X = 'Raven (TAQ)' ;
X = 'Rippletail (SC)' ;
X = 'Roach' ;
X = 'Slatefur' ;
X = 'Sleekwhisker' ;
X = 'Sparrowtail' ;
X = 'Spikefur' ;
X = 'Thistle (Ro)' ;
X = 'Yarrowleaf'.
```

## Some failed attempts

```prolog
% Attempts to find out all the cats whose parents belong to different Clans.
% Failed because they might be in StarClan now.

?- child(X, F, M), belongsto(F, FC), belongsto(M, MC), FC \== MC.
X = 'Berrynose',
F = 'Smoky',
M = 'Daisy',
FC = 'Loner',
MC = 'ThunderClan' ;
X = 'Blossomheart',
F = 'Sharpclaw (SC)',
M = 'Cherrytail',
FC = 'SkyClan',
MC = 'Loner' ;
X = 'Bramble',
F = 'Splinter',
M = 'Milkweed',
FC = 'Slash\'s Group',
MC = 'ThunderClan' ;
X = 'Cinderfur',
F = 'Clawface',
M = 'Rowanberry',
FC = 'Place of No Stars',
MC = 'StarClan' ;
...
```

