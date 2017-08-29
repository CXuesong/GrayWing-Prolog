% Derived rules

child(X, Y) :- child(X, Y, _); child(X, _, Y).
son(X, Y) :- male(X), child(X, Y).
daughter(X, Y) :- female(X), child(X, Y).

parent(X, Y) :- child(Y, X).
father(X, Y) :- male(X), parent(X, Y).
mother(X, Y) :- female(X), parent(X, Y).

grandchild(X, Y) :- child(X, PARENT), child(PARENT, Y).
grandson(X, Y) :- male(X), grandchild(X, Y).
granddaughter(X, Y) :- female(X), grandchild(X, Y).

grandparent(X, Y) :- grandchild(Y, X).
grandfather(X, Y) :- male(X), grandparent(X, Y).
grandmother(X, Y) :- female(X), grandparent(X, Y).

sibling(X, Y) :- child(X, FATHER, MOTHER), child(Y, FATHER, MOTHER), X \== Y.
brother(X, Y) :- male(X), sibling(X, Y).
sister(X, Y) :- female(X), sibling(X, Y).

halfsibling(X, Y) :- (
        (child(X, FATHER, MOTHER1), child(Y, FATHER, MOTHER2), MOTHER1 \== MOTHER2);
        (child(X, FATHER1, MOTHER), child(Y, FATHER2, MOTHER), FATHER1 \== FATHER2)
    ), X \== Y.
halfbrother(X, Y) :- male(X), halfsibling(X, Y).
halfsister(X, Y) :- female(X), halfsibling(X, Y).

