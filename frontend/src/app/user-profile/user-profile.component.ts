import {Component, OnInit} from '@angular/core';
import {MyAuthService} from "../services/MyAuthService";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Observable} from "rxjs";
import {PropertiesGetAllResponse} from "../home/properties-getall-response";
import {User} from "./edit-user";
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../confirmation-dialog/confirmation-dialog.component';



@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit{

  constructor(public myAuthService: MyAuthService, public router :Router, private httpClient: HttpClient, private dialog: MatDialog) {
  }
  selectedFile: File | null = null;
  user: User = {};
  tempUser: User = {};
  isEditing: boolean = false;
  properties:PropertiesGetAllResponse[] = [];
  reviews:any;
  showSuccessAlert = false;
  showFailAlert = false;

  hideAlert(): void {
    this.showSuccessAlert = false;
    this.showFailAlert = false;
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }
  uploadProfileImage(): void {
    const formData = new FormData();
    formData.append('image', this.selectedFile as Blob);

    const userId = this.myAuthService.returnId();
    if (!userId) {
      console.error('User ID not found');
      return;
    }

    const url = `${MojConfig.adresa_servera}/UploadProfileImage?id=${userId}`;

    this.httpClient.post(url, formData).subscribe(
      response => {
        console.log('Profile image uploaded successfully', response);
        this.getUserData(userId).subscribe(data => {
          this.user = data;
          this.tempUser = { ...data };
        });
      },
      error => {
        console.error('Error uploading profile image', error);
      }
    );
  }

  getProfileImage(userId: number | undefined): void {
    const url = `${MojConfig.adresa_servera}/GetProfileImage?id=${userId}`;

    this.httpClient.get(url, { responseType: 'text' }).subscribe(
      (data: string) => {
        if (data) {
          this.user.imagePath = data;
        } else {
          console.log('No image found');
        }
      },
      (error) => {
        console.error('Error fetching profile image:', error);
      }
    );
  }
  toggleEditMode(el: HTMLElement) {
    el.scrollIntoView({behavior: 'smooth'});
    this.isEditing = !this.isEditing;
  }
  ngOnInit(): void {
    const userId = this.myAuthService.returnId();
    if (userId) {
      this.getUserData(userId).subscribe(data => {
        this.user = data;
        this.tempUser = { ...data };
        this.getProfileImage(userId);
      }, error => {
        console.error('Error fetching user data', error);
      });
      this.getReservations();
    } else {
      console.error('User ID not found');
    }


    this.user = {
      name: '',
      surname: '',
      username:'',
      phone: '',
      email: '',
      dateBirth: '',
      dateOfRegistraion: '',
      imagePath: '',
      newPassword: '',
      confirmPassword: ''
    };
    }
  hidePassword1 = true;
  hideConfirmPassword = true;
  password:any;
  noReservations = true;

  getUserData(id: number): Observable<any> {
    const userId = this.myAuthService.returnId();
    let url = MojConfig.adresa_servera+`/GetUserById?id=${userId}`;
    if (userId) {
      return this.httpClient.get<any>(url);
    }
    throw new Error('User ID not found');
  }

  logOut() {

    let token = window.localStorage.getItem("my-auth-token")??"";
    window.localStorage.setItem("my-auth-token","");

    let url=MojConfig.adresa_servera+`/Autentifikacija/logout`
    this.httpClient.post(url, {}, {
      headers:{
        "my-auth-token": token
      }
    }).subscribe(x=>{
    })

    this.router.navigate(["/login"])
  }

  scroll(el: HTMLElement) {
    el.scrollIntoView({behavior: 'smooth'});
  }
  nazad(){
    this.router.navigate(["/home"]);
  }
  saveChanges(): void {
    if (!this.user) return;
    if (this.user.newPassword && this.user.confirmPassword) {
      if (this.user.newPassword !== this.user.confirmPassword) {

       this.showFailAlert = true;
        return;
      }
    }

    const userId = this.myAuthService.returnId();
    if (!userId) {
      console.error('User ID not found');
      return;
    }
    const updatedUser = {
      ...this.user,
      newPassword: this.user.newPassword,
    };

    const url = `${MojConfig.adresa_servera}/EditUser?id=${userId}`;

    this.httpClient.put(url, updatedUser).subscribe(
      (response: any) => {
        this.showSuccessAlert = true;
        setTimeout(() => this.showSuccessAlert = false, 3000);
        this.getUserData(userId).subscribe(data => {
          this.user = data;
          this.tempUser = { ...data };
          this.getProfileImage(userId);
          this.isEditing = false;
        }, error => {
          console.error('Error fetching updated user data', error);
        });
      },
      (error) => {
        console.error('Error updating user data', error);
      }
    );
  }

  userReservations: any[] = [];

  getReservations(): void {
    const userId = this.myAuthService.returnId();
    if (!userId) {
      console.error("User ID not found");
      return;
    }
  
    let url = `${MojConfig.adresa_servera}/api/Reservations/GetByUserId/${userId}`;
  
    this.httpClient.get<any[]>(url).subscribe(
      (response: any[] | null) => {
        if (!response || response.length === 0) {
          this.userReservations = [];
          this.noReservations = true;
          return;
        }
        this.userReservations = response;
        this.noReservations = false;
      },
      error => {
        console.error('Error fetching user reservations', error);
      }
    );
  }


  reservationToCancel: number | null = null;

  openCancelDialog(reservationId: number): void {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent, {
      width: '300px', 
      data: { reservationId } 
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.cancelReservation(reservationId);
      }
    });
  }

  closeDialog(): void {
    this.dialog.closeAll();
  }

  cancelReservation(reservationId: number): void {
    const userId = this.myAuthService.returnId();  
    if (!userId) {
      console.error("User ID not found");
      return;
    }
  
    const url = `${MojConfig.adresa_servera}/api/Reservations/Delete/${reservationId}`;
  
    this.httpClient.delete(url, { responseType: 'text' }).subscribe(
      (response) => {
        console.log('Reservation canceled:', response);  
        this.userReservations = this.userReservations.filter(reservation => reservation.id !== reservationId);
  
        if (this.userReservations.length === 0) {
          this.noReservations = true;
        }
        
        
      },
      (error) => {
        console.error('Error canceling reservation', error);
      }
    );
  }
  
  
  
  
  
  
}
