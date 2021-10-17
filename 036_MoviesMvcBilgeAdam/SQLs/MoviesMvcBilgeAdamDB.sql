delete from MovieDirectors
delete from Reviews
delete from Movies
delete from Directors

dbcc CHECKIDENT ('MovieDirectors', RESEED, 0)
dbcc CHECKIDENT ('Reviews', RESEED, 0)
dbcc CHECKIDENT ('Movies', RESEED, 0)
dbcc CHECKIDENT ('Directors', RESEED, 0)

insert into Directors values (N'James', N'Cameron', 0)
insert into Directors values (N'Guy', N'Ritchie', 0)
insert into Directors values (N'F. Gary', N'Gray', 0)

insert into Movies values (N'Avatar', N'2009', 1000000)
insert into Movies values (N'Sherlock Holmes', N'2009', NULL)
insert into Movies values (N'Law Abiding Citizen', N'2009', 300000)
insert into Movies values (N'Aliens', N'1986', 10000000)

insert into Reviews values (N'Very good movie.', 9, N'Çağıl Alsaç', '2021-02-21', 1)

insert into MovieDirectors values (1, 1)
insert into MovieDirectors values (2, 2)
insert into MovieDirectors values (3, 3)
insert into MovieDirectors values (4, 1)