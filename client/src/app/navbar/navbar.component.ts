import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { User } from '../_models/user';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  model :any = {}
  //currentUser$: Observable<User | null>
  // loggedIn: boolean | undefined;

  constructor(public accountService: AccountService, private route:Router, private toastr: ToastrService) { }

  ngOnInit(): void {
    //this.currentUser$ = this.accountService.currentUser$;
    // this.getCurrentUser();
  }

  login() {
    this.accountService.login(this.model).subscribe(response=> {
      this.route.navigateByUrl("/members");
      // this.loggedIn = true;
    }, error => {
      this.toastr.error(error.error);
      console.log(error);
    });
  }

  logout() {
    this.accountService.logout();
    this.route.navigateByUrl("/");
    // this.loggedIn = false;
  }

  // getCurrentUser() {
  //   this.accountService.currentUser$.subscribe(user => {
  //     this.loggedIn = !!user;
  //   }, error=> {
  //     console.log(error);
  //   })
  // }

}
