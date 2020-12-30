﻿using Fakebook.Profile.Domain;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Fakebook.Profile.UnitTests.TestData
{
    /// <summary>
    /// return randomized user profiles in the correct format
    /// </summary>
    /// <returns></returns>
        public static class UserData
        {
            public static class Create
            {
                /*
                 * User:
                 * - Email: string
                 * - ProfilePictureUrl: string 
                 * - Name : string
                 * - FirstName: string
                 * - Lastname: string              
                 * - PhoneNumber: string
                 * - BirthDate: DateTime
                 * - Status: string
                 */

                public class Valid : IEnumerable<object[]>
                {
                    /// <summary>
                    /// generates 4 variants of valid user profiles:
                    /// profile picture, status
                    ///        -           -
                    ///        +           +
                    ///        +           -
                    ///        -           +
                    /// </summary>
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new DomainProfile
                        {
                            Email = GenerateRandom.Email(),                          
                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = null,                       
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = null,
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {
                            Email = GenerateRandom.Email(),                            
                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String(),
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {
                            Email = GenerateRandom.Email(),
                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = null,
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {
                            Email = GenerateRandom.Email(),
                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = null,
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String(),
                        }
                        };
                    }
                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }

                /// <summary>
                /// generates 3 variants of invalid user profiles:
                /// invalid first name, last name
                /// invalid email format
                /// invlaid phone number format 
                /// </summary>
                public class Invalid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new DomainProfile
                        {
                            Email = GenerateRandom.Email(),     
                            FirstName = null,
                            LastName = null,
                            ProfilePictureUrl = GenerateRandom.String(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String(),
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {
                            Email = GenerateRandom.Email(),
                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            PhoneNumber = GenerateRandom.String(), // .PhoneNumber()
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String(),
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {
                            Email = GenerateRandom.String(), // .Email()
                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            PhoneNumber = GenerateRandom.PhoneNumber(), 
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String(),
                        }
                        };
                    }
                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }
            }

            /// <summary>
            /// placeholder
            /// </summary>          
            public static class ReadByEmail
            {
                public class Valid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        string target = GenerateRandom.Email();

                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = target,
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },
                        target
                        };

                        target = GenerateRandom.Email();
                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = target,
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            },
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            },
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },
                        target
                        };
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }

                public class Invalid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },
                        GenerateRandom.Email()
                        };

                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },
                        null
                        };

                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },
                        GenerateRandom.String()
                        };
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }
            }

            public static class ReadByIds
            {
                public class Valid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },
                        new List<int>{ 1 }
                        };

                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            },
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            },
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },
                        new List<int>
                        {
                            GenerateRandom.Int(1, 3),
                            GenerateRandom.Int(1, 3)
                        }
                        };
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }

                public class Invalid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },
                        new List<int> { -1, 4 }
                        };
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }
            }

            public static class Update
            {

                public class Valid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new DomainProfile
                        {
 
                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = null,
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = null
                        },
                        new DomainProfile
                        {

                            FirstName =  GenerateRandom.String(),
                            LastName =  GenerateRandom.String(),
                            ProfilePictureUrl = null,
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String()
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {

                            FirstName =  GenerateRandom.String(),
                            LastName =  GenerateRandom.String(),
                            ProfilePictureUrl = null,
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String()
                        },
                        new DomainProfile
                        {

                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = null,
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = null
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {

                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = null
                        },
                        new DomainProfile
                        {

                            FirstName =  GenerateRandom.String(),
                            LastName =  GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String()
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {

                            FirstName =  GenerateRandom.String(),
                            LastName =  GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String()
                        },
                        new DomainProfile
                        {

                            FirstName = GenerateRandom.String(),
                            LastName = GenerateRandom.String(),
                            ProfilePictureUrl = null,
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = null
                        }
                        };
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }

                public class Invalid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new DomainProfile
                        {
                            FirstName =  GenerateRandom.String(),
                            LastName =  GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String()
                        },
                        new DomainProfile
                        {
                            FirstName =  null,
                            LastName =  null,
                            ProfilePictureUrl = GenerateRandom.String(),
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String()
                        }
                        };

                        yield return new object[]
                        {
                        new DomainProfile
                        {                        
                            FirstName =  GenerateRandom.String(),
                            LastName =  GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            Email = GenerateRandom.Email(),
                            PhoneNumber = GenerateRandom.PhoneNumber(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String()
                        },
                        new DomainProfile
                        {                            
                            FirstName =  GenerateRandom.String(),
                            LastName =  GenerateRandom.String(),
                            ProfilePictureUrl = GenerateRandom.String(),
                            Email = GenerateRandom.String(),
                            PhoneNumber = GenerateRandom.String(),
                            BirthDate = GenerateRandom.DateTime(),
                            Status = GenerateRandom.String()
                        }
                        };
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }
            }

            public static class Delete
            {
                public class Valid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },

                        1
                        };

                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            },
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            },
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },

                        GenerateRandom.Int(1, 3)
                        };
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }

                public class Invalid : IEnumerable<object[]>
                {
                    public IEnumerator<object[]> GetEnumerator()
                    {
                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },

                        -1
                        };

                        yield return new object[]
                        {
                        new List<DomainProfile>
                        {
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            },
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            },
                            new DomainProfile
                            {
                                FirstName = GenerateRandom.String(),
                                LastName = GenerateRandom.String(),
                                ProfilePictureUrl = null,
                                Email = GenerateRandom.Email(),
                                PhoneNumber = GenerateRandom.PhoneNumber(),
                                BirthDate = GenerateRandom.DateTime(),
                                Status = null
                            }
                        },

                        4
                        };
                    }

                    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                }
            }
        }
    }
}