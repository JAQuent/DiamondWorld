# WD
setwd("E:/research_projects/OLM_project/experiment/unityProjects/DiamondWorld/nonUnity_folder")

# Params
totalNumberOfRays <- 25
margin            <- 0.05

raysPerDim        <- sqrt(totalNumberOfRays)
from              <- margin
to                <- 1 - margin

# Create x and y values
x <- seq(from = from, to = to, length = raysPerDim)
y <- x

# Combine all possibilities
rayCoordinates <- expand.grid(x, y)



# Lib
library(ggplot2)


ggplot(rayCoordinates, aes(x = Var1, y = Var2)) + geom_point()

paste0("[", paste0(rayCoordinates$Var1, collapse = ", "), "]")
paste0("[", paste0(rayCoordinates$Var2, collapse = ", "), "]")