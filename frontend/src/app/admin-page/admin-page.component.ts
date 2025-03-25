import { Component, OnInit } from '@angular/core';
import { MyAuthService } from '../services/MyAuthService';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { PropertiesGetAllResponse } from '../home/properties-getall-response';
import { MojConfig } from '../moj-config';
import { User } from '../home/properties-getall-response';
import { AddPropertyDialogComponent } from '../add-property-dialog/add-property-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { CreatePropertyDto } from '../add-property-dialog/add-property-dialog.component';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent implements OnInit {
  userName: string = '';
  isAdmin: boolean = false;
  properties: PropertiesGetAllResponse[] = [];
  users: User[] = []; 
  showProperties: boolean = false;
  showUsers: boolean = false;

  property: any = {
    name: '',
    adress: '',
    numberOfRooms: 0,
    numberOfBathrooms: 0,
    pricePerNight: 0,
    city: { name: '' },
    propertyType: { name: '' },
    images: [],
    reviews: []
  };

  constructor(
    private myAuthService: MyAuthService,
    private router: Router,
    private httpClient: HttpClient,
    private dialog: MatDialog,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    const user = this.myAuthService.getUser();
    if (user) {
      this.userName = user.username;
      this.isAdmin = user.isAdmin;
    } else {
      this.router.navigate(['/login']);
    }
  }

  getProperties() {
    const url = `${MojConfig.adresa_servera}/api/Properties/GetAll`;
    this.httpClient.get<PropertiesGetAllResponse[]>(url).subscribe(
      (response: PropertiesGetAllResponse[]) => {
        this.properties = response;
      },
      (error) => {
        console.error('Error fetching properties:', error);
      }
    );
  }
  
  

  getUsers() {
    const url = `${MojConfig.adresa_servera}/api/User/GetAll`;
    this.httpClient.get<User[]>(url).subscribe(
      (response: User[]) => {
        this.users = response;
      },
      (error) => {
        console.error('Error fetching users:', error);
      }
    );
  }

  onPropertiesClick() {
    this.showUsers = false;
    this.showProperties = true; 
    this.getProperties();
  }

  onUsersClick() {
    this.showProperties = false;
    this.showUsers = true; 
    this.getUsers();
  }

  getUserImageSrc(profileImage: string): string {
    return profileImage ? profileImage : 'https://placehold.co/100x100';
  }

  openAddPropertyDialog(): void {
    const dialogRef = this.dialog.open(AddPropertyDialogComponent, {
      width: '400px'
    });

    dialogRef.afterClosed().subscribe((result: CreatePropertyDto | undefined) => {
      if (result) {
        this.addProperty(result);
      }
    });
  }

  addProperty(property: CreatePropertyDto): void {
    const url = `${MojConfig.adresa_servera}/api/Properties/Add`;
    this.httpClient.post(url, property).subscribe(() => {
      this.getProperties();
    });
  }

  deleteProperty(propertyId: number): void {
    const url = `${MojConfig.adresa_servera}/api/Properties/Delete/${propertyId}`;
    this.httpClient.delete(url).subscribe(() => {
      this.getProperties();
      window.location.reload();
    }, (error) => {
      console.error('Error deleting property:', error);
    });
  }

  deleteUser(userId: number):void {
    const url = `${MojConfig.adresa_servera}/api/User/Delete/${userId}`;

    this.httpClient.delete(url).subscribe(() => {
      window.location.reload();
      this.getProperties();
    }, (error) => {
      console.error('Error deleting user:', error);
    });
  }

  logout(): void {
    this.myAuthService.logout();
    this.router.navigate(['/login']);
  }

  loadPropertyImages(propertyId: number): void {
    const url = `${MojConfig.adresa_servera}/api/Properties/GetPropertyImages?propertyId=${propertyId}`;
    this.httpClient.get<string[]>(url).subscribe(
      (data) => {
        const updatedProperties = this.properties.map((property) => {
          if (property.id === propertyId) {
            property.images = data.map((imagePath, index) => ({
              id: index + 1,
              path: imagePath
            }));
          }
          return property;
        });
        this.properties = updatedProperties;
      },
      (error) => {
        console.error('Error fetching property images', error);
      }
    );
  }
  
  
  

  getPropertyImagePath(image: { id: number, path: string }): string {
    return image && image.path ? image.path : 'https://placehold.co/100x100';
  }

  uploadImage(event: any, propertyId: number): void {
    const files = event.target.files;
  

    if (files && files[0]) {
      const file = files[0];
      const formData = new FormData();
      formData.append('image', file);
      formData.append('id', propertyId.toString());
  
      const url = `${MojConfig.adresa_servera}/UploadPropertyImage?id=${propertyId}`;
  
      this.httpClient.post(url, formData).subscribe(
        (response) => {
          this.snackBar.open('Image uploaded successfully!', 'Close', { duration: 3000 });
          this.loadPropertyImages(propertyId);
        },
        (error) => {
          console.error('Error uploading image', error);
          this.snackBar.open('Failed to upload image. Please try again.', 'Close', { duration: 3000 });
        }
      );
    } else {
      this.snackBar.open('No image selected', 'Close', { duration: 3000 });
    }
  }
  
  
  
  
  
}
