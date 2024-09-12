import {Component, OnInit} from '@angular/core';
import {MyAuthService} from "../services/MyAuthService";
import {Router} from "@angular/router";
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../moj-config";
import {Observable} from "rxjs";
import {PropertiesGetAllResponse} from "../home/properties-getall-response";
import {User} from "./edit-user";

@Component({
  selector: 'user-profile',
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent implements OnInit{

  constructor(public myAuthService: MyAuthService, public router :Router, private httpClient: HttpClient) {
  }
  selectedFile: File | null = null;
  user: User = {};
  tempUser: User = {};
  isEditing: boolean = false;
  properties:PropertiesGetAllResponse[] = [];
  reviews:any;
  showSuccessAlert = false;

  hideAlert(): void {
    this.showSuccessAlert = false;
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
          this.user.imagePath = `data:image/jpeg;base64,${data}`;
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
        this.getReviews();
        //console.log(this.user);
      }, error => {
        console.error('Error fetching user data', error);
      });
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
  noReviews= true;
  noReservations = true;

  getReviews(): void {
    const userId = this.myAuthService.returnId();
    let url = MojConfig.adresa_servera+`/GetReviewByUserId?userId=${userId}`;
    console.log("Request URL:", url);
    this.httpClient.get<any[]>(url).subscribe(
      (data) => {
        if (data === null) {
          this.noReviews = true;
        } else {
          this.reviews = data;
          this.noReviews = false;
        }
      },
      (error) => {
        console.error('Error fetching reviews:', error);
      }
    );
  }



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
      console.log("logout uspjesan")
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
        console.log('Passwords do not match');
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
        //console.log('User updated successfully');
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
  getReservations() {
    let url = MojConfig.adresa_servera + '/api/Properties/GetAll';
    console.log('Fetching properties from URL:', url);

    this.httpClient.get<PropertiesGetAllResponse[]>(url).subscribe(
      (x: PropertiesGetAllResponse[]) => {
        this.properties = x;
      },
      error => {
        console.error('Error fetching properties', error);
      }
    );

  }
}
