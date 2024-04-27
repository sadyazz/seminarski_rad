import { Component } from '@angular/core';

@Component({
  selector: 'home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  showDropdown: boolean = false;

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }

  brojacA =0;
  brojacC =0;
  uvecajA(){
    this.brojacA++;
  }

  smanjiA(){
    if(this.brojacA>0)
      this.brojacA--;
  }
  uvecajC(){
    this.brojacC++;
  }

  smanjiC(){
    if(this.brojacC>0)
      this.brojacC--;
  }
}
