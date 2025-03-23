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

  constructor(
    private myAuthService: MyAuthService,
    private router: Router,
    private httpClient: HttpClient,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    const user = this.myAuthService.getUser();
    if (user) {
      this.userName = user.username;
      this.isAdmin = user.isAdmin;
    } else {
      this.router.navigate(['/login']); // Redirect to login if no user is logged in
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
  
}
