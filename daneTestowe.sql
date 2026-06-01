INSERT INTO "Addresses" ("City", "Street", "Building", "Apartment", "ZipCode") VALUES ('Warszawa', 'Marszałkowska', '1', NULL, '00-001');
INSERT INTO "Addresses" ("City", "Street", "Building", "Apartment", "ZipCode") VALUES ('Kraków', 'Floriańska', '12', '3', '31-002');
INSERT INTO "Addresses" ("City", "Street", "Building", "Apartment", "ZipCode") VALUES ('Gdańsk', 'Długa', '45', NULL, '80-003');
INSERT INTO "Addresses" ("City", "Street", "Building", "Apartment", "ZipCode") VALUES ('Poznań', 'Święty Marcin', '78', '12A', '61-004');
INSERT INTO "Addresses" ("City", "Street", "Building", "Apartment", "ZipCode") VALUES ('Wrocław', 'Rynek', '25', '7', '50-005');
INSERT INTO "Addresses" ("City", "Street", "Building", "Apartment", "ZipCode") VALUES ('Łódź', 'Piotrkowska', '89', NULL, '90-006');
INSERT INTO "Addresses" ("City", "Street", "Building", "Apartment", "ZipCode") VALUES ('Katowice', 'Mariacka', '34', '5', '40-007');




INSERT INTO "States" ("Name") VALUES ('Available');
INSERT INTO "States" ("Name") VALUES ('Borrowed');
INSERT INTO "States" ("Name") VALUES ('Lost');



INSERT INTO "Jobs" ("Name") VALUES ('Director');
INSERT INTO "Jobs" ("Name") VALUES ('Librarian');
INSERT INTO "Jobs" ("Name") VALUES ('Assistant');
INSERT INTO "Jobs" ("Name") VALUES ('Technician');
INSERT INTO "Jobs" ("Name") VALUES ('Administrator');


INSERT INTO "Publishers" ("Name") VALUES ('Wydawnictwo Literackie');
INSERT INTO "Publishers" ("Name") VALUES ('Prószyński i S-ka');
INSERT INTO "Publishers" ("Name") VALUES ('Znak');
INSERT INTO "Publishers" ("Name") VALUES ('Muza SA');
INSERT INTO "Publishers" ("Name") VALUES ('Wydawnictwo Albatros');




INSERT INTO "Genres" ("Name", "ChildrenFriendly") VALUES ('Science Fiction', 0);
INSERT INTO "Genres" ("Name", "ChildrenFriendly") VALUES ('Fantasy', 1);
INSERT INTO "Genres" ("Name", "ChildrenFriendly") VALUES ('Kryminał', 0);
INSERT INTO "Genres" ("Name", "ChildrenFriendly") VALUES ('Horror', 0);
INSERT INTO "Genres" ("Name", "ChildrenFriendly") VALUES ('Romans', 0);
INSERT INTO "Genres" ("Name", "ChildrenFriendly") VALUES ('Literatura piękna', 0);
INSERT INTO "Genres" ("Name", "ChildrenFriendly") VALUES ('Bajki', 1);
INSERT INTO "Genres" ("Name", "ChildrenFriendly") VALUES ('Popularnonaukowa', 0);



INSERT INTO "Languages" ("Name") VALUES ('pol');
INSERT INTO "Languages" ("Name") VALUES ('ang');
INSERT INTO "Languages" ("Name") VALUES ('esp');
INSERT INTO "Languages" ("Name") VALUES ('ros');




INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('Andrzej', 'Sapkowski');
INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('Stanisław', 'Lem');
INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('Jacek', 'Dukaj');
INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('Olga', 'Tokarczuk');
INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('Remigiusz', 'Mróz');
INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('Katarzyna', 'Bonda');
INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('Stephen', 'King');
INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('J.R.R.', 'Tolkien');
INSERT INTO "Authors" ("FirstName", "LastName") VALUES ('George R.R.', 'Martin');


INSERT INTO "Emps" ("AddressId", "DeptId", "JobId", "FirstName", "LastName") VALUES (4, 1, 1, 'Anna', 'Kowalska');
INSERT INTO "Emps" ("AddressId", "DeptId", "JobId", "FirstName", "LastName") VALUES (5, 1, 2, 'Piotr', 'Nowak');
INSERT INTO "Emps" ("AddressId", "DeptId", "JobId", "FirstName", "LastName") VALUES (6, 2, 1, 'Maria', 'Wiśniewska');
INSERT INTO "Emps" ("AddressId", "DeptId", "JobId", "FirstName", "LastName") VALUES (7, 2, 2, 'Tomasz', 'Lewandowski');
INSERT INTO "Emps" ("AddressId", "DeptId", "JobId", "FirstName", "LastName") VALUES (4, 3, 1, 'Ewa', 'Dąbrowska');
INSERT INTO "Emps" ("AddressId", "DeptId", "JobId", "FirstName", "LastName") VALUES (5, 3, 3, 'Kamil', 'Kamiński');




INSERT INTO "Depts" ("AddressId", "DirectorId") VALUES (1, 1);
INSERT INTO "Depts" ("AddressId", "DirectorId") VALUES (2, 3);
INSERT INTO "Depts" ("AddressId", "DirectorId") VALUES (3, 5);


INSERT INTO "Readers" ("AddressId", "FirstName", "LastName") VALUES (1, 'Jan', 'Kowalski');
INSERT INTO "Readers" ("AddressId", "FirstName", "LastName") VALUES (2, 'Agnieszka', 'Nowak');
INSERT INTO "Readers" ("AddressId", "FirstName", "LastName") VALUES (3, 'Michał', 'Wiśniewski');
INSERT INTO "Readers" ("AddressId", "FirstName", "LastName") VALUES (4, 'Katarzyna', 'Lewandowska');
INSERT INTO "Readers" ("AddressId", "FirstName", "LastName") VALUES (5, 'Paweł', 'Zieliński');




INSERT INTO "Books" ("Title", "Isbn", "PublisherId") VALUES ('Wiedźmin - Ostatnie życzenie', '9788375781234', 1);
INSERT INTO "Books" ("Title", "Isbn", "PublisherId") VALUES ('Solaris', '9788376480581', 2);
INSERT INTO "Books" ("Title", "Isbn", "PublisherId") VALUES ('Lód', '9788324034567', 3);
INSERT INTO "Books" ("Title", "Isbn", "PublisherId") VALUES ('Księgi Jakubowe', '9788308082345', 4);
INSERT INTO "Books" ("Title", "Isbn", "PublisherId") VALUES ('Inne Pieśni', '9788376481236', 2);
INSERT INTO "Books" ("Title", "Isbn", "PublisherId") VALUES ('Władca Pierścieni', '9788328734561', 5);
INSERT INTO "Books" ("Title", "Isbn", "PublisherId") VALUES ('Gra o Tron', '9788324045678', 1);
INSERT INTO "Books" ("Title", "Isbn", "PublisherId") VALUES ('Lśnienie', '9788376480895', 2);


INSERT INTO "BookAuthors" ("BookId", "AuthorId") VALUES (1, 1);
INSERT INTO "BookAuthors" ("BookId", "AuthorId") VALUES (2, 2);
INSERT INTO "BookAuthors" ("BookId", "AuthorId") VALUES (3, 3);
INSERT INTO "BookAuthors" ("BookId", "AuthorId") VALUES (4, 4);
INSERT INTO "BookAuthors" ("BookId", "AuthorId") VALUES (5, 3);
INSERT INTO "BookAuthors" ("BookId", "AuthorId") VALUES (6, 8);
INSERT INTO "BookAuthors" ("BookId", "AuthorId") VALUES (7, 9);
INSERT INTO "BookAuthors" ("BookId", "AuthorId") VALUES (8, 7);


INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (1, 2);
INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (2, 1);
INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (3, 1);
INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (4, 6);
INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (5, 2);
INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (6, 2);
INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (7, 2);
INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (8, 3);
INSERT INTO "BookGenres" ("BookId", "GenreId") VALUES (8, 4);


INSERT INTO "BookLanguages" ("BookId", "LanguageId") VALUES (1, 1);
INSERT INTO "BookLanguages" ("BookId", "LanguageId") VALUES (2, 1);
INSERT INTO "BookLanguages" ("BookId", "LanguageId") VALUES (3, 1);
INSERT INTO "BookLanguages" ("BookId", "LanguageId") VALUES (4, 1);
INSERT INTO "BookLanguages" ("BookId", "LanguageId") VALUES (5, 1);
INSERT INTO "BookLanguages" ("BookId", "LanguageId") VALUES (6, 1);
INSERT INTO "BookLanguages" ("BookId", "LanguageId") VALUES (7, 1);
INSERT INTO "BookLanguages" ("BookId", "LanguageId") VALUES (8, 1);



INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (1, 1, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (1, 1, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (1, 2, 2);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (2, 1, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (2, 2, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (3, 1, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (3, 3, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (4, 2, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (5, 1, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (5, 3, 2);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (6, 1, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (6, 2, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (6, 3, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (7, 1, 1);
INSERT INTO "Copies" ("BookId", "DeptId", "StateId") VALUES (8, 2, 1);



INSERT INTO "Borrows" ("CopyId", "ReaderId", "BorrowDate", "ExpectedReturnDate", "ReturnDate", "TimesExtended") 
VALUES (1, 1, DATE '2024-01-15', DATE '2024-02-15', DATE '2024-02-10', 0);

INSERT INTO "Borrows" ("CopyId", "ReaderId", "BorrowDate", "ExpectedReturnDate", "ReturnDate", "TimesExtended") 
VALUES (3, 2, DATE '2024-02-01', DATE '2024-03-01', NULL, 1);

INSERT INTO "Borrows" ("CopyId", "ReaderId", "BorrowDate", "ExpectedReturnDate", "ReturnDate", "TimesExtended") 
VALUES (5, 3, DATE '2024-02-10', DATE '2024-03-10', NULL, 0);

INSERT INTO "Borrows" ("CopyId", "ReaderId", "BorrowDate", "ExpectedReturnDate", "ReturnDate", "TimesExtended") 
VALUES (10, 4, DATE '2024-01-20', DATE '2024-02-20', DATE '2024-02-18', 0);

INSERT INTO "Borrows" ("CopyId", "ReaderId", "BorrowDate", "ExpectedReturnDate", "ReturnDate", "TimesExtended") 
VALUES (14, 5, DATE '2024-02-15', DATE '2024-03-15', NULL, 0);


COMMIT;