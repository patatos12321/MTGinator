import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'players',
  templateUrl: './players.component.html'
})


export class PlayersComponent {

  public players: Player[];
  public newPlayer: Player;
  public error: string;
  public loading: boolean;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.loading = true;

    this.ResetNewPlayer();
    this.RefreshData();
  }

  RefreshData() {
    this.http.get<Player[]>(this.baseUrl + 'api/players').subscribe(result => {
      this.players = result;
      this.loading = false;
    }, error => console.error(error));
  }

  ResetNewPlayer() {
    this.newPlayer = {
      name: null,
      score: 0,
      winLossRatio : 0.0,
      id: 0,
      isInEditMode: false
    };
  }

  DeletePlayer(id: number) {
    this.loading = true;

    this.http.delete(this.baseUrl + "api/players/" + id).subscribe(result => {
      this.RefreshData();
    }, error => console.error(error));
  }

  EditPlayer(player: Player) {
    this.loading = true;

    this.http.put(this.baseUrl + "api/players/" + player.id, player).subscribe(result => {
      this.RefreshData();
    }, error => console.error(error));
  }

  AddNewPlayer() {
    if (this.newPlayer.name == null || this.newPlayer.name == "") {
      this.error = "The player name can't be empty";
      return;
    }

    this.error = null;

    this.loading = true;
    this.players.push(this.newPlayer);

    this.http.post(this.baseUrl + "api/players", this.players).subscribe(result => {
      this.RefreshData();
    }, error => console.error(error));

    this.ResetNewPlayer();
  }
}

interface Player {
  name: string;
  score: number;
  winLossRatio: number;
  id: number;
  isInEditMode: boolean;
}
