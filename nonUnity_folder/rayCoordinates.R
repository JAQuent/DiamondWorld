# WD
setwd("~/Unity/UXF_tracker_sampleProject/input_data")

# Params
totalNumberOfRays <- 100
margin            <- 0.05

raysPerDim        <- sqrt(totalNumberOfRays)
from              <- margin
to                <- 1 - margin

# Create x and y values
x <- seq(from = from, to = to, length = raysPerDim)
y <- x

# Combine all possibilities
rayCoordinates <- expand.grid(x, y)

write.table(rayCoordinates, 'rayCoordinates.txt', quote = FALSE, sep = '\t', row.names = FALSE, col.names = FALSE)


# Lib
library(ggplot2)


ggplot(rayCoordinates, aes(x = Var1, y = Var2)) + geom_point()
