> ### DISCLAIMER
> **IMPORTANT:** The current state of this sample application is **BETA**, consider it version a 0.1, therefore, many areas could be improved and change significantly. This is purely built with the purpose of showing off a concept at this point. 

# RecommendationEngine 
Recommendation Engine using Azure ML Studio to recommend related movies. The app relies on using the popular [MovieLens 20M dataset](https://grouplens.org/datasets/movielens/20m/). 

The Azure Machine Learning model was built using the [TrainBox Match Recommender](https://msdn.microsoft.com/en-us/library/azure/dn905987.aspx) which is a hybrid recommender. It uses both user-content and colloborative filtering to provide recommendations out-of-the-box

You can find more details about the app and [how the Azure Machine Learning recommendation model is built through this blog](https://blogs.msdn.microsoft.com/dotnet/?p=10746&preview=true)


## How to get this running!
   * Clone the repo and open the [movierecommender.sln](https://github.com/dotnet-architecture/RecommendationEngine/blob/master/movierecommender.sln) and build in Visual Studio 2017. Currently the app is only tested for Windows. 
   * Start with this [MovieLens Movie Recommendation model](https://gallery.cortanaintelligence.com/Experiment/MovieLens-Movie-Recommender), this is based on the 1M dataset, you will need to replace the datasets to use the 20M dataset for better results. 
   * After publishing your webservice, change the **'apikey' and 'uri'** in the [appsettings.json](https://github.com/dotnet-architecture/RecommendationEngine/blob/master/movierecommender/appsettings.json) file with your webservice keys instead. 

# Feedback on this app
As mentioned, we'd appreciate to your feedback, improvements and ideas. You can create new issues at the issues section, do pull requests and/or send emails to aasthan@microsoft.com
