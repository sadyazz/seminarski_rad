/*
export interface User {
  name: string;
  surname: string;
  username: string;
  phone: string;
  email: string;
  dateBirth: string;
  dateOfRegistraion: string;
}*/
export interface User {
  name?: string;
  surname?: string;
  username?:string;
  email?: string;
  phone?: string;
  dateBirth?: string;
  dateOfRegistraion?: string;
 newPassword?: string;
  confirmPassword?: string;
}
