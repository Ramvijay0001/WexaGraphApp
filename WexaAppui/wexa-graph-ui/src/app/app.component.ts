import { Component } from '@angular/core';
// import { RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { GraphService } from './services/graph.service';
import { GraphViewComponent } from './components/graph-view/graph-view.component';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule,GraphViewComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
   technology = '';

  projects: string[] = [];

  domains: any[] = [];

  recommendations: any[] = [];

  loading = false;

  searched = false;
selectedNode: any = null;
  errorMessage = '';

  constructor(
    private graphService: GraphService
  ) {}
graphData: any[] = [];

search(): void {

  this.errorMessage = '';
  this.searched = false;
  this.selectedNode = null;

  const technology = this.technology.trim();

  if (!technology) {

    this.errorMessage =
      'Please enter a technology.';

    return;
  }

  this.loading = true;

  this.projects = [];
  this.domains = [];
  this.recommendations = [];
  this.graphData = [];

  this.graphService
    .getProjectsByTechnology(technology)
    .subscribe({

      next: (response) => {

        this.projects =
          response.projects ?? [];

        this.loadDomains(technology);
      },

      error: (error) => {

        console.error(error);

        this.loading = false;

        this.errorMessage =
          'Unable to connect to the API.';
      }
    });
}
private loadGraph(
  technology: string
): void {

  this.graphService
    .getGraph(technology)
    .subscribe({

      next: (response) => {

        this.graphData =
          response.graph ?? [];

        this.loading = false;
        this.searched = true;

        if (this.graphData.length === 0) {

          this.errorMessage =
            `No graph data found for "${technology}".`;
        }
      },

      error: (error) => {

        console.error(error);

        this.loading = false;

        this.errorMessage =
          'Failed to load graph data.';
      }
    });
}
  private loadDomains(technology: string): void {

    this.graphService
      .getTechnologyDomains(technology)
      .subscribe({
        next: (response) => {

          this.domains = response.results ?? [];

          this.loadRecommendations(technology);
        },
        error: (error) => {

          console.error(error);

          this.errorMessage =
            'Failed to load domain information.';

          this.loading = false;
        }
      });
  }

private loadRecommendations(technology: string): void {

  this.graphService
    .getRecommendations(technology)
    .subscribe({

      next: (response) => {

        this.recommendations =
          response.recommendations ?? [];

        // Next step: Load graph
        this.loadGraph(technology);
      },

      error: (error) => {

        console.error(error);

        this.loading = false;

        this.errorMessage =
          'Failed to load recommendations.';
      }
    });
}
  onNodeSelected(node: any): void {

  this.selectedNode = node;

}
}
