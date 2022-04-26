# Script to run generate input data for DiamondWorld
# Version 1.0
# Date:  04/04/2022
# Author: Joern Alexander Quent
# /* 
# ----------------------------- Note on this script ---------------------------
# */
#
# /* 
# ----------------------------- Libraries, settings and functions ---------------------------
# */
######################################################
setwd("~/Work/resarchProjects/OLM_project/experiment/unityProjects/DiamondWorld/nonUnity_folder")
######################################################

# Libs
library(plyr)
library(ggplot2)

# Seed
set.seed(20220404)

# /* 
# ----------------------------- Params ---------------------------
# */
# Spawn limits
x_min <- -45
x_max <- 45
z_min <- -45
z_max <- 45

# Number of objects
numDiamonds <- 20
numIcosahedrons <- 50

# Reactivation range
time_min <- 10
time_max <- 30

# /* 
# ----------------------------- Draw random locations ---------------------------
# */
diamond_x <- round(runif(numDiamonds, min = x_min, max = x_max), 1)
diamond_z <- round(runif(numDiamonds, min = z_min, max = z_max), 1)
diamondTimings <- round(runif(numDiamonds, min = time_min, max = time_max), 1)

icosahedron_x <- round(runif(numIcosahedrons, min = x_min, max = x_max), 1)
icosahedron_z <- round(runif(numIcosahedrons, min = z_min, max = z_max), 1)
icosahedronTimings <- round(runif(numIcosahedrons, min = time_min, max = time_max), 1)


# /* 
# ----------------------------- Visualisation ---------------------------
# */
# Add to one dataframe
df <- data.frame(object = c(rep('diamond', numDiamonds), rep('icosahedron', numIcosahedrons)),
                 x = c(diamond_x, icosahedron_x),
                 z = c(diamond_z, icosahedron_z),
                 timer = c(diamondTimings, icosahedronTimings))

vis <- ggplot(data = df, aes(x = x, y = z, colour = object, size = timer)) + 
  geom_point() +
  labs(title = 'Diamond & icosahedron distribution', x = "", y = "")

vis
ggsave("objectDistribution.png", vis)


# /* 
# ----------------------------- Create output ---------------------------
# */

paste0("[", paste0(diamond_x, collapse = ", "), "]")
paste0("[", paste0(diamond_z, collapse = ", "), "]")
paste0("[", paste0(diamondTimings, collapse = ", "), "]")
paste0("[", paste0(icosahedron_x, collapse = ", "), "]")
paste0("[", paste0(icosahedron_z, collapse = ", "), "]")
paste0("[", paste0(icosahedronTimings, collapse = ", "), "]")