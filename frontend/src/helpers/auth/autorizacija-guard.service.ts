import {Injectable} from "@angular/core";
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from "@angular/router";
import {MyAuthService} from "../../app/services/MyAuthService";


@Injectable()
export class AutorizacijaGuard implements CanActivate {

  constructor(private router: Router, private myAuthService: MyAuthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {


    if (this.myAuthService.jelLogiran()) {

      let isUser = this.myAuthService.isUser();

      if (isUser)
      {
       /* this.router.navigate(['/login']);
        return false;*/
        return true;
      }

      this.router.navigate(['/login']);
      return false;
    }


    // not logged in so redirect to login page with the return url
    this.router.navigate(['/login'], { queryParams: { povratniUrl: state.url }});
    return false;
  }
}
