/*
Stan na dzien 27.06.2022)
*/
USE Hotel;

INSERT INTO Gosc(Imie, Nazwisko, Telefon, Email)
VALUES('Jan', 'Kowalski', '505666505', 'jankowalski@gmail.com');
INSERT INTO Gosc(Imie, Nazwisko, Telefon, Email)
VALUES('Jakub', 'Nowak', '666585447', 'kubanowak@gmail.com');
INSERT INTO Gosc(Imie, Nazwisko, Telefon)
VALUES('Stefan', 'Zbigniew', '505666505');
INSERT INTO Gosc(Imie, Nazwisko, Telefon, Email)
VALUES('Wiktor', 'Bożydar', '505835489', 'wikibozydar@gmail.com');
INSERT INTO Gosc(Imie, Nazwisko, Telefon, Email)
VALUES('Ilona', 'Spleśniała', '534470778', 'ilonkaaa@gmail.com');
INSERT INTO Gosc(Imie, Nazwisko, Telefon, Email)
VALUES('Krzysztof', 'Wyczesany', '601735601', 'wyczesanyb@gmail.com');
INSERT INTO Gosc(Imie, Nazwisko, Telefon)
VALUES('Bogdan', 'Gamoń', '532461700');
INSERT INTO Gosc(Imie, Nazwisko, Telefon, Email)
VALUES('Zbigniew', 'Grzywacz', '698493280', 'zbysiugrzywacz@gmail.com');
INSERT INTO Gosc(Imie, Nazwisko, Telefon)
VALUES('Stanisław', 'Ulanda', '511820691');
INSERT INTO Gosc(Imie, Nazwisko, Telefon, Email)
VALUES('Filip', 'Baran', '723091319', 'filipbaran666@gmail.com');

INSERT INTO Pokoj VALUES('0', '001', '3', 249.99);
INSERT INTO Pokoj VALUES('0', '002', '4', 299.99);
INSERT INTO Pokoj VALUES('1', '101', '1', 199.99);
INSERT INTO Pokoj VALUES('1', '102', '1', 199.99);
INSERT INTO Pokoj VALUES('1', '103', '2', 224.99);
INSERT INTO Pokoj VALUES('1', '104', '2', 224.99);
INSERT INTO Pokoj VALUES('2', '201', '1', 199.99);
INSERT INTO Pokoj VALUES('2', '202', '4', 299.99);
INSERT INTO Pokoj VALUES('2', '203', '2', 224.99);
INSERT INTO Pokoj VALUES('2', '204', '3', 249.99);

INSERT INTO Rezerwacja VALUES('2022-03-15', '2022-03-18', 1);
INSERT INTO Rezerwacja VALUES('2021-12-15', '2021-12-16', 4);
INSERT INTO Rezerwacja VALUES('2022-04-02', '2022-04-04', 3);
INSERT INTO Rezerwacja VALUES('2022-01-28', '2022-01-29', 2);
INSERT INTO Rezerwacja VALUES('2022-01-28', '2022-01-29', 5);
INSERT INTO Rezerwacja VALUES('2021-12-03', '2021-12-04', 7);
INSERT INTO Rezerwacja VALUES('2021-12-20', '2021-12-22', 6);
INSERT INTO Rezerwacja VALUES('2021-12-31', '2022-01-01', 8);
INSERT INTO Rezerwacja VALUES('2022-01-01', '2022-01-06', 10);
INSERT INTO Rezerwacja VALUES('2022-01-08', '2022-01-11', 9);

INSERT INTO Pobyt VALUES(6, 4);
INSERT INTO Pobyt VALUES(2, 7);
INSERT INTO Pobyt VALUES(7, 6);
INSERT INTO Pobyt VALUES(8, 5);
INSERT INTO Pobyt VALUES(9, 1);
INSERT INTO Pobyt VALUES(3, 10);
INSERT INTO Pobyt VALUES(10, 2);
INSERT INTO Pobyt VALUES(4, 3);
INSERT INTO Pobyt VALUES(5, 8);
INSERT INTO Pobyt VALUES(1, 9);
