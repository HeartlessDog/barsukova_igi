using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab3IGI
{
        public static class DbInitializer
        {
            public static void Initialize(LibraryContext context)
            {
                List<string> names;
                Random rand = new Random();
                RandomDateTime date = new RandomDateTime();

                if (!context.Jenres.Any())
                {
                    names = new List<string>(new string[] {
                "Non-fiction", "Fiction", "Poetry", "Autobiographies", "Biographies", "History", "Science", "Drama", "Mystery", "Horror"
            });

                    foreach (string name in names)
                    {
                        context.Jenres.Add(new Jenre
                        {
                            Name = name,
                            Description = ""
                        });
                    }

                    context.SaveChanges();
                }

                if (!context.Books.Any())
                {
                    names = new List<string>(new string[] {
                "Book1", "Book2", "Book3", "Book4", "Book5", "Book6", "Book7", "Book8", "Book9", "Book10"
            });
                    foreach (string name in names)
                    {
                        context.Books.Add(new Book
                        {
                            RegistrationNumber = rand.Next(100, 1000),
                            Name = name,
                            Author = "Author N.",
                            Edition = "Edition T.",
                            YearOfEdition = rand.Next(1990, 2018),
                            JenreID = rand.Next(1, 10),
                            Price = rand.Next(100, 500),

                        });
                    }

                    context.SaveChanges();
                }

                if (!context.Readers.Any())
                {

                    names = new List<string>(new string[] {
                "Reader1", "Reader2", "Reader3","Reader4","Reader5"
            });
                    foreach (string name in names)
                    {
                        context.Readers.Add(new Reader
                        {
                            FullName = name,
                            DateOfBirth = date.Next(),
                            Sex = "",
                            Adres = "",
                            PhoneNumber = "",
                            PassportData = ""
                        });
                    }

                    context.SaveChanges();
                }

                if (!context.Issuances.Any())
                {

                    for (int i = 0; i < 20; i++)
                    {
                        context.Issuances.Add(new Issuence
                        {
                            ReaderID = rand.Next(1, 5),
                            BookID = rand.Next(1, 10),
                            DateOfIssuance = date.Next()
                        });
                    }
                    context.SaveChanges();
                }

            }
        }
        public class RandomDateTime
        {
            DateTime start;
            Random gen;
            int range;

            public RandomDateTime()
            {
                start = new DateTime(1995, 1, 1);
                gen = new Random();
                range = (DateTime.Today - start).Days;
            }

            public DateTime Next()
            {
                return start.AddDays(gen.Next(range)).AddHours(gen.Next(0, 24)).AddMinutes(gen.Next(0, 60)).AddSeconds(gen.Next(0, 60));
            }
        }
    }

