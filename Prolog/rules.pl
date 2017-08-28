% Derived rules

child(X, Y) :- child(X, Y, _); child(X, _, Y).
son(X, Y) :- male(X), child(X, Y).
daughter(X, Y) :- female(X), child(X, Y).

parent(X, Y) :- son(Y, X).
father(X, Y) :- male(X), parent(X, Y).
mother(X, Y) :- female(X), parent(X, Y).

grandchild(X, Y) :- child(X, PARENT), child(PARENT, Y).
grandson(X, Y) :- male(X), grandchild(X, Y).
granddaughter(X, Y) :- female(X), grandchild(X, Y).

grandparent(X, Y) :- grandson(Y, X).
grandfather(X, Y) :- male(X), grandparent(X, Y).
grandmother(X, Y) :- female(X), grandparent(X, Y).
